using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Domain.Entities.UserEntities.AuthEntities;
using Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence.EntityFramework;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    private readonly ICurrentUser _currentUser;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    #region overrides

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.AppendGlobalQueryFilter<SoftDeletableEntity<long>>(e => e.IsActive);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.PrepareAddedEntities(_currentUser);
        ChangeTracker.PrepareModifiedEntities(_currentUser);
        //ChangeTracker.PrepareDeletedEntities(_isHardDelete);

        return await base.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region db sets

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
    public DbSet<SelectedTeam> UserFavoriteTeams { get; set; }
    public DbSet<Sponsor> Sponsors { get; set; }
    public DbSet<Championship> Championshipss { get; set; }
    public DbSet<ChampionSponsor> ChampionSponsors { get; set; }
    public DbSet<TeamSponsor> TeamSponsors { get; set; }

    #endregion
}
