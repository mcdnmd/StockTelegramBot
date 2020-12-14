using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class PostgresqlDbContext : DbContext
    {
        public DbSet<UserRecord> UserRecords { get; set; }
        
        public PostgresqlDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = "Host=ec2-46-137-100-204.eu-west-1.compute.amazonaws.com;" +
                                "Port=5432;" +
                                "Database=d6vnb72j4dek12;" +
                                "Username=didfiwmrzmtckk;" +
                                "Password=404d1bf7deaae50eb132bce2b3cf1e87b00928438befb9a65f3dcad9054be82c;" +
                                "SSL Mode=Require; " +
                                "Trust Server Certificate=true;";
            optionsBuilder.UseNpgsql(connectionStr);
        }
    }
}