using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class PostgresqlDbContext : DbContext
    {
        public DbSet<UserRecord> UserRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = "Host=ec2-54-246-87-132.eu-west-1.compute.amazonaws.com;" +
                                "Port=5432;" +
                                "Database=d5ceu5kcjnqaf8;" +
                                "Username=rntavfhtxconyj;" +
                                "Password=24e77ae837de198e01c45b9ef6fb451a4cd205a957596140bcddbe6e08893aaf;" +
                                "SSL Mode=Require; " +
                                "Trust Server Certificate=true;";
            optionsBuilder.UseNpgsql(connectionStr);
        }
    }
}