using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace Infrastructure{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
         
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("");
        }
    }
    public class PostgreHandler<T> : IDataBase<T>
    {
        public void Add(T obj)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(T obj)
        {
            throw new System.NotImplementedException();
        }

        public void Select(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}