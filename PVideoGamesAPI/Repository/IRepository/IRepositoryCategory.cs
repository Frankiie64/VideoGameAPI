using PVideoGamesAPI.Models;
using PVideoGamesAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Repository.IRepository
{
    public interface IRepositoryCategory
    {
        public Task<List<Category>> GetCategories();
        public Task<Category> GetCategory(int id);
        public Task<bool> CreateCategory(Category category);
        public Task<bool> UpdateCategory( Category category);
        public Task<bool> DeleteCategory(Category category);
        public Task<bool> ExistCategory(int id);
        public Task<bool> ExistCategory(string name);
        public Task<bool> Save();

    }
}
