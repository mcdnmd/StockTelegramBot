using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;

namespace TlgImitation.DbImitation
{
    public class PostgreHandlerTest : IDataBase
    {
        private readonly PostgresqlDbContextTest _dbContextTest = new PostgresqlDbContextTest();
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

        public Task<List<UserRecord>> GetAllUsers()
        {
            throw new System.NotImplementedException();
        }
    }
}