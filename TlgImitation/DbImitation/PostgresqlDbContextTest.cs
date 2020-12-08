using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TlgImitation.DbImitation
{
    public class PostgresqlDbContextTest : DbContext
    {
        public DbSet<UserRecord> UserRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = "Host=ec2-54-246-87-132.eu-west-1.compute.amazonaws.com;" +
                                "Port=5432;" +
                                "Database=d9cpseuak8n0d6;" +
                                "Username=ivzlrgkviovnbq;" +
                                "Password=176adf81b9734b44fe603b5873e1fb23b224dedb893dd9a60a7f4536e2cf2c8f;" +
                                "SSL Mode=Require; " +
                                "Trust Server Certificate=true;";
            optionsBuilder.UseNpgsql(connectionStr);
        }
    }
}