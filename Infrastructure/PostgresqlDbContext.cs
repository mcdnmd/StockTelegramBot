using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class PostgresqlDbContext : DbContext
    {
        internal PostgresqlDbContext(string connectionName, DbContextOptions options)
            : base(options ?? new DbContextOptions<PostgresqlDbFactory>())
        {
            CurrentConnectionName = connectionName;
        }

        private string CurrentConnectionName { get; }

        public DbSet<UserRecord> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = GetConnectionString();
            optionsBuilder.UseNpgsql(connectionStr);
        }

        private string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            return configuration.GetConnectionString(CurrentConnectionName);
        }
    }
}