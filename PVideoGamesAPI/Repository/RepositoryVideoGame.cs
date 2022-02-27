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
    public class RepositoryVideoGame : IRepositoryVideoGame
    {
        private readonly ApplicationDbContext _db;

        public RepositoryVideoGame(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<Game>> GetVideoGames()
        {
            return await _db.Game.OrderBy(v => v.Title).Include(ca => ca.Category)
                .Include(re => re.requirements).ToListAsync();

        }
        public async Task<Game> GetVideoGame(int id)
        {
            return await _db.Game.Include(ca => ca.Category).Include(re => re.requirements).FirstOrDefaultAsync(v => v.Id == id);
                
        }
        public async Task<List<Game>> GetVideoGamesInCate(int IdCateg)
        {
            return await _db.Game.Where(ca => ca.IdCategory == IdCateg).ToListAsync();
        }
        public async Task<IEnumerable<Game>> FindVideoGames(string Keyword)
        {
            IQueryable<Game> query =  _db.Game;

            if (!string.IsNullOrEmpty(Keyword))
            {
                query = query.Where(e => e.Title.Contains(Keyword) || e.Sumary.Contains(Keyword));
            }
            return await query.Include(ca => ca.Category).Include(re => re.requirements).ToListAsync();
        }
        
        public async  Task<bool> ExistVideoGame(int id)
        {
           return await _db.Game.AnyAsync(g => g.Id == id);
            
        }

        public async Task<bool> ExistVideoGame(string Title)
        {
            return await _db.Game.AnyAsync(g => g.Title.ToLower().Trim() == Title.ToLower().Trim());
        }


        public async Task<bool> CreateVideoGame(Game Game)
        {
           await _db.Game.AddAsync(Game);
            return await Save();
        }

        public async Task<bool> DeleteVideoGame(Game Game)
        {
             _db.Game.Remove(Game);
            return await Save();
        }
        public async Task<bool> UpdateVideoGame(Game Game)
        {
             _db.Game.Update(Game);
            return await Save();
        }

        public async Task<bool> Save()
        {
          return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

     
    }
}

