using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DataBase
{
    public interface IDataBase
    {
        public Task<UserRecord> FindUser(long id);

        public Task<UserRecord> AddNewUser(UserRecord userRecord);

        public Task<UserRecord> UpdateUser(UserRecord userRecord);

        public Task<UserRecord> RemoveUser(UserRecord userRecord);

        public Task<List<UserRecord>> GetAllUsers();
    }
}