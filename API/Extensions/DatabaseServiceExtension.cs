using Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class DatabaseServiceExtension
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration, DatabaseType databaseType = DatabaseType.SqlServer)
        {
            string sqlServerconnectionString = configuration.GetConnectionString("SqlServerConnection") ??
                throw new InvalidOperationException("Connection string 'SqlServerConnection' not found.");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlServerconnectionString).EnableSensitiveDataLogging(true).EnableDetailedErrors());
            //if(databaseType == DatabaseType.MySql)
            //{
            //    string mySQLConnectionString = configuration.GetConnectionString("MySqlConnection") ??
            //        throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");

            //    //services.AddDbContext<AppDbContext>(options =>
            //    //    options.UseMySql(mySQLConnectionString, ServerVersion.AutoDetect(mySQLConnectionString)));
            //    //services.AddDbContextFactory<AppDbContext>(options =>
            //    //    options.UseMySql(mySQLConnectionString, ServerVersion.AutoDetect(mySQLConnectionString)));
            //}
            //else if (databaseType == DatabaseType.SqlServer)
            //{
            //    //services.AddDbContextFactory<AppDbContext>(options => options.UseSqlServer(sqlServerconnectionString));
            //}
        }
    }

    public enum DatabaseType
    {
        MySql = 1,
        SqlServer = 2
    }
}
