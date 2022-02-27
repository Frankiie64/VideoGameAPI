using Microsoft.EntityFrameworkCore;
using PVideoGamesAPI.Data;
using PVideoGamesAPI.Models;
using PVideoGamesAPI.Models.Dtos;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Repository
{
    public class RepositoryCategory : IRepositoryCategory
    {
        private readonly ApplicationDbContext _db;

        public RepositoryCategory(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await _db.category.AddAsync(category);
            return await Save();
        }
        public async Task<bool> UpdateCategory(Category category)
        {
            _db.category.Update(category);
            return await Save();
        }
        public async Task<bool> DeleteCategory(Category category)
        {
            _db.category.Remove(category);
            return await Save();
        }

        public async Task<bool> ExistCategory(int id)
        {
            return await _db.category.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> ExistCategory(string name)
        {
            return await _db.category.AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _db.category.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category> GetCategory(int Id)
        {
            return await _db.category.FirstOrDefaultAsync(c => c.Id == Id);
        }
     
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
            
        }
    }
}
