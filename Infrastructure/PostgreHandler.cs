using System;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PostgreHandler : IDataBase
    {
        private readonly PostgresqlDbContext _dbContextTest = new PostgresqlDbContext();
        public async Task<UserRecord> FindUser(long id) => await _dbContextTest.UserRecords.FindAsync(id);

        public async Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            await _dbContextTest.UserRecords.AddAsync(userRecord);
            await _dbContextTest.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            _dbContextTest.UserRecords.Update(userRecord);
            await _dbContextTest.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            _dbContextTest.UserRecords.Remove(userRecord);
            await _dbContextTest.SaveChangesAsync();
            return userRecord;
        }
    }
}