using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;

namespace TlgImitation
{
    public class MockDataBase : IDataBase
    {
        public Dictionary<long, UserRecord> Users = new Dictionary<long, UserRecord>();
        public async Task<UserRecord> FindUser(long id)
        {
            if (Users.ContainsKey(id))
            {
                return Users[id];
            }
            return null;
        }

        public async Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            var id = userRecord.Id;
            Users[id] = userRecord;
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            return userRecord;
        }

        public async Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            if (Users.ContainsKey(userRecord.Id))
            {
                Users.Remove(userRecord.Id);
                return userRecord;
            }
            return null;
        }
    }
}