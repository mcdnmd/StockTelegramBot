using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DataBase;


namespace Infrastructure.DataBase
{
    public class PostgreHandler : IDataBase
    {
        private readonly PostgresqlDbContext dbContext = new PostgresqlDbContext();

        public async Task<UserRecord> FindUser(long id)
        {
            return await dbContext.UserRecords.FindAsync(id);
        }

        public async Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            
            await dbContext.UserRecords.AddAsync(userRecord);
            await dbContext.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            dbContext.UserRecords.Update(userRecord);
            await dbContext.SaveChangesAsync();
            return userRecord;
        }

        public async Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            dbContext.UserRecords.Remove(userRecord);
            await dbContext.SaveChangesAsync();
            return userRecord;
        }

        public async Task<List<UserRecord>> GetAllUsers()
        {
            return dbContext.UserRecords.ToList();
        }
    }
}