using Domain.Entities;
using Domain.Entities.CartEntity;
using Domain.Entities.UserEntities;
using Domain.Entities.UserEntities.AuthEntities;
using Domain.Enums;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastructure.Persistence.EntityFramework;

public class AppDbContext : IdentityDbContext<ApplicationUser, Role, string>
{
    private bool _isSoftDeleteFilterDisabled;
    private IEnumerable<Claim> _currentUserClaims = new List<Claim>();

    protected IHttpContextAccessor? HttpContextAccessor { get; }

    [ActivatorUtilitiesConstructor]
    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        HttpContextAccessor = httpContextAccessor;
        var httpContext = HttpContextAccessor?.HttpContext;
        _currentUserClaims = httpContext?.User.Claims ?? new List<Claim>();
    }

    public AppDbContext() : base() { }


    public void SwitchSoftDeleteFilter(bool disabled = true) => _isSoftDeleteFilterDisabled = disabled;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<BaseEntity>()
            .HasQueryFilter(e => e.SoftDeleteCount >= 0 || _isSoftDeleteFilterDisabled)
            .UseTpcMappingStrategy();

        // ApplicationUser and other custom entities
        builder.Entity<ApplicationUser>()
            .HasQueryFilter(e => e.SoftDeleteCount >= 0 || _isSoftDeleteFilterDisabled)
            .HasMany(u => u.FavoriteTeams);

        builder.Entity<RefreshToken>()
            .ToTable(nameof(RefreshTokens).Pluralize())
            .HasBaseType<BaseEntity>()
            .Property(x => x.Id);

        builder.Entity<ApplicationUser>()
            .HasOne(p => p.Cart)
            .WithOne(i => i.User)
            .HasForeignKey<ApplicationUser>(b => b.CartId);

        // Configure UserFavoriteTeam relationship with Team
        builder.Entity<UserFavoriteTeam>()
            .HasOne(uft => uft.Team)
            .WithMany(t => t.UserFavoriteTeams)
            .HasForeignKey(uft => uft.TeamId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure Ticket relationship with Team
        builder.Entity<Ticket>()
            .HasOne(ti => ti.Team)
            .WithMany(t => t.Tickets)
            .HasForeignKey(ti => ti.TeamId)
            .OnDelete(DeleteBehavior.NoAction);

        // **Order -> CartItem Relationship**
        builder.Entity<Order>()
            .HasMany(oi => oi.CartItems)
            .WithOne(o => o.Order)
            .HasForeignKey(oi => oi.OrderId);

        // **CartItem -> Ticket Relationship**
        builder.Entity<CartItem>()
            .HasOne(ci => ci.Ticket)
            .WithMany()  // Assuming no direct collection of CartItems in Ticket
            .HasForeignKey(ci => ci.TicketId);

        base.OnModelCreating(builder);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Block> Blocks { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Stadium> Stadiums { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<UserFavoriteTeam> UserFavoriteTeams { get; set; }

    public void PrepareEntity()
    {
        try
        {

            var entities = ChangeTracker.Entries().Where(ee => ee.Entity is BaseEntity || ee.Entity is ApplicationUser)
                                                  .ToDictionary(ee => ee, ee => ee.Entity as dynamic);
            bool isArchived = entities.Any(e => e.Key.State == EntityState.Deleted && e.Value.BaseEntityStatus == BaseEntityStatus.Archived);
            bool isRestored = entities.Any(e => e.Key.State == EntityState.Deleted && e.Value.BaseEntityStatus == BaseEntityStatus.Restored);

            foreach (var entity in entities)
            {
                if (entity.Key.State == EntityState.Deleted)
                {
                    if (isArchived || isRestored)
                    {
                        if (entity.Value.BaseEntityStatus == (BaseEntityStatus.Archived | BaseEntityStatus.Restored)) entity.Value.SoftDeleteCount = 0;

                        if (isArchived)
                            entity.Value.SoftDeleteCount = (entity.Value.SoftDeleteCount > 0) ? -1 : --entity.Value.SoftDeleteCount;
                        else if (isRestored)
                            entity.Value.SoftDeleteCount = (entity.Value.SoftDeleteCount >= 0) ? 0 : ++entity.Value.SoftDeleteCount;

                        entity.Key.State = EntityState.Modified;
                        entity.Value.BaseEntityStatus = null;
                        entity.Value.ModifiedDate = DateTime.Now;
                        entity.Value.ModifiedBy = (_currentUserClaims != null && _currentUserClaims.Any(x => x.Type == "Id")) ?
                            new Guid(_currentUserClaims.FirstOrDefault(x => x.Type == "Id")!.Value) : Guid.Empty;
                    }
                }
                else if (entity.Key.State == EntityState.Added)
                {
                    //if (entity.Value is ApplicationUser)
                    //{
                    //    entity.Value.Id = Guid.NewGuid().ToString();
                    //}
                    //else
                    //{
                    //    entity.Value.Id = Guid.NewGuid();
                    //}
                    entity.Value.BaseEntityStatus = null;
                    entity.Value.SoftDeleteCount = 0;
                    entity.Value.CreatedDate = DateTime.Now;
                    entity.Value.CreatedBy = (_currentUserClaims != null && _currentUserClaims.Any(x => x.Type == "Id")) ?
                        new Guid(_currentUserClaims.FirstOrDefault(x => x.Type == "Id")!.Value) : Guid.Empty;
                }
                else if (entity.Key.State == EntityState.Modified)
                {
                    entity.Value.BaseEntityStatus = null;
                    entity.Value.SoftDeleteCount = 0;
                    entity.Value.ModifiedDate = DateTime.Now;
                    entity.Value.ModifiedBy = (_currentUserClaims != null && _currentUserClaims.Any(x => x.Type == "Id")) ?
                        new Guid(_currentUserClaims.FirstOrDefault(x => x.Type == "Id")!.Value) : Guid.Empty;
                }
            }
        }
        catch
        {
            throw;
        }
    }

    public override int SaveChanges()
    {
        try
        {
            PrepareEntity();
            int results = base.SaveChanges();
            _isSoftDeleteFilterDisabled = false;
            return results;
        }
        catch
        {
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            PrepareEntity();
            int results = await base.SaveChangesAsync(cancellationToken);
            _isSoftDeleteFilterDisabled = false;
            return results;
        }
        catch
        {
            throw;
        }
    }
}
