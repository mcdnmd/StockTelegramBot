using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class PostgresqlDbFactory : DbContext
    {
        private const string ProductionConnectionName = "Production";
        private const string TestConnectionName = "Test";

        public PostgresqlDbContext CreateDbContext(string[] args) => UseTestConnection();

        private static PostgresqlDbContext UseTestConnection(DbContextOptions options = null)
            => new PostgresqlDbContext(TestConnectionName, options);

        private static PostgresqlDbContext UseProductionConnection(DbContextOptions options = null)
            => new PostgresqlDbContext(ProductionConnectionName, options);
    }
}