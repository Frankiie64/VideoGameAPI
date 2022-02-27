using Microsoft.EntityFrameworkCore;
using PVideoGamesAPI.Data;
using PVideoGamesAPI.Models.Tables_Complements;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Repository
{
    public class RepositoryRequirements : IRepositoryRequirements
    {
        private readonly ApplicationDbContext _db;
        public RepositoryRequirements(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<Requeriments>> GetRequirements()
        {
            return await _db.requeriments.OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<Requeriments> GetRequiremnt(int id)
        {
            return await _db.requeriments.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> CreateRequiremnts(Requeriments requiremnts)
        {
            await _db.requeriments.AddAsync(requiremnts);
            return await Save();
        }
        public async Task<bool> UpdateRequiremnts(Requeriments requiremnts)
        {
             _db.requeriments.Update(requiremnts);
            return await Save();
        }
        public async Task<bool> DeleteRequiremnts(Requeriments requiremnts)
        {
             _db.requeriments.Remove(requiremnts);
            return await Save();
        }

        public async Task<bool> ExistRequiremnts(string os)
        {
            return await _db.requeriments.AnyAsync(r => r.Os == os);
        }

        public async Task<bool> ExistRequiremnts(int id)
        {
            return await _db.requeriments.AnyAsync(r => r.Id == id);
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

       
    }
}
