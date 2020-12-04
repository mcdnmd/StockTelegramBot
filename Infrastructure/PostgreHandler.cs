using System;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PostgreHandler : IDataBase
    {
        private readonly PostgresqlDbContext _dbContext =
            new PostgresqlDbFactory().CreateDbContext(null);
        public async Task<UserRecord> FindUser(long id) => await _dbContext.FindAsync<UserRecord>(id);

        public async Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            await _dbContext.AddAsync(userRecord);
            await _dbContext.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            _dbContext.Update(userRecord);
            await _dbContext.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            _dbContext.Remove(userRecord);
            await _dbContext.SaveChangesAsync();
            return userRecord;
        }
    }
}