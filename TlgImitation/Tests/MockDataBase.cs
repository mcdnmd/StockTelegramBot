using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;

namespace TlgImitation
{
    public class MockDataBase : IDataBase
    {
        public Dictionary<long, UserRecord> dataBase = new Dictionary<long, UserRecord>();
        public async Task<UserRecord> FindUser(long id)
        {
            if (dataBase.ContainsKey(id))
            {
                return dataBase[id];
            }
            return null;
        }

        public async Task<UserRecord> AddNewUser(UserRecord userRecord)
        {
            var id = userRecord.Id;
            dataBase[id] = userRecord;
            return userRecord;
        }

        public async Task<UserRecord> UpdateUser(UserRecord userRecord)
        {
            return userRecord;
        }

        public async Task<UserRecord> RemoveUser(UserRecord userRecord)
        {
            if (dataBase.ContainsKey(userRecord.Id))
            {
                dataBase.Remove(userRecord.Id);
                return userRecord;
            }
            return null;
        }
    }
}