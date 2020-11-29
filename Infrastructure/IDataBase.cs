using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IDataBase
    {
        public Task<UserDto> FindUser(long id);

        public Task<UserDto> AddNewUser(UserDto userDto);

        public Task<UserDto> UpdateUser(UserDto userDto);

        public Task<UserDto> RemoveUser(UserDto userDto);
    }
}