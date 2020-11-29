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
        public async Task<UserDto> FindUser(long id) => await _dbContext.FindAsync<UserDto>(id);

        public async Task<UserDto> AddNewUser(UserDto userDto)
        {
            await _dbContext.AddAsync(userDto);
            await _dbContext.SaveChangesAsync();
            return userDto;
        }

        public async Task<UserDto> UpdateUser(UserDto userDto)
        {
            _dbContext.Update(userDto);
            await _dbContext.SaveChangesAsync();
            return userDto;
        }

        public async Task<UserDto> RemoveUser(UserDto userDto)
        {
            _dbContext.Remove(userDto);
            await _dbContext.SaveChangesAsync();
            return userDto;
        }
    }
}