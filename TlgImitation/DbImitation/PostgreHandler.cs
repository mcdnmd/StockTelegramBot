using System;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PostgreHandler : IDataBase
    {
        private readonly PostgresqlDbContext _dbContext = new PostgresqlDbContext();
        public async Task<UserRecord> FindUser(long id) => await _dbContext.UserRecords.FindAsync(id);

        public async Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            await _dbContext.UserRecords.AddAsync(userRecord);
            await _dbContext.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            _dbContext.UserRecords.Update(userRecord);
            await _dbContext.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            _dbContext.UserRecords.Remove(userRecord);
            await _dbContext.SaveChangesAsync();
            return userRecord;
        }
    }
}