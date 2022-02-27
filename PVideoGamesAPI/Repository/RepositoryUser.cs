using Microsoft.EntityFrameworkCore;
using PVideoGamesAPI.Data;
using PVideoGamesAPI.Models;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Repository
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly ApplicationDbContext _db;
        public RepositoryUser(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<User>> GetUsers()
        {
            return await _db.user.OrderBy(u => u.Nickname).ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _db.user.FirstOrDefaultAsync(u => u.Id == id);

        }
        public async Task<bool> DeleteUser(User user)
        {
             _db.user.Remove(user);
            return await Save();
        }


        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHasd, passwordSalt;

            CrearPassWordHash(password, out passwordHasd, out passwordSalt);

            user.PasswordHash = passwordHasd;
            user.PasswordSalt = passwordSalt;

            await _db.AddAsync(user);
            
            await Save();

            return user;
        }

        public async Task<bool> UpdateUser(User user,string password)
        {
            byte[] passwordHasd, passwordSalt;

            CrearPassWordHash(password, out passwordHasd, out passwordSalt);

            user.PasswordHash = passwordHasd;
            user.PasswordSalt = passwordSalt;

            _db.user.Update(user);
            return await Save();

        }

        public async Task<bool> ExistUser(string nickname)
        {
            return await _db.user.AnyAsync(u => u.Nickname.ToLower().Trim() == nickname.ToLower().Trim());
        }
        public async Task<bool> ExistUser(int id)
        {
            return await _db.user.AnyAsync(u => u.Id == id);
        }

        public async Task<User> Login(string password, string nickname)
        {
            User item = await _db.user.FirstOrDefaultAsync(x => x.Nickname == nickname);

            if (item == null)
            {
                return null;
            }

            if (!validarPasswordHash(password, item.PasswordHash, item.PasswordSalt))
            {
                return null;
            }

            return item;
        }

        
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }


        #region Helper

        //metodos predefinidos para su uso
        private bool validarPasswordHash(string password, byte[] passwordHasd, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int x = 0; x < hashComputado.Length; x++)
                {
                    if (hashComputado[x] != passwordHasd[x]) return false;
                }
            }
            return true;
        }
        private void CrearPassWordHash(string password, out byte[] passwordHasd, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHasd = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

   
        #endregion
    }
}
