using PVideoGamesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Repository.IRepository
{
    public interface IRepositoryUser
    {
        public Task<List<User>> GetUsers();
        public Task<User> GetUser(int id);
        public Task<bool> UpdateUser(User user,string password);
        public Task<bool> DeleteUser(User user);
        public Task<bool> ExistUser(string nickname);
        public Task<bool> ExistUser(int id);

        public Task<User> Login(string password, string nickname);
        public Task<User> Register(User user,string password);

        public Task<bool> Save();


    }
}
