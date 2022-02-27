using PVideoGamesAPI.Models.Tables_Complements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Repository.IRepository
{
    public interface IRepositoryRequirements
    {
        public Task<List<Requeriments>> GetRequirements();
        public Task<Requeriments> GetRequiremnt(int id);
        public Task<bool> CreateRequiremnts(Requeriments requiremnts);
        public Task<bool> UpdateRequiremnts(Requeriments requiremnts);
        public Task<bool> DeleteRequiremnts(Requeriments requiremnts);
        public Task<bool> ExistRequiremnts(string os);
        public Task<bool> ExistRequiremnts(int id);

        public Task<bool> Save();
    }
}
