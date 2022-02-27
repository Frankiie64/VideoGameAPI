using PVideoGamesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Repository.IRepository
{
    public interface IRepositoryVideoGame
    {
        public Task<List<Game>> GetVideoGames();
        public Task<Game> GetVideoGame(int id);
        public Task<List<Game>> GetVideoGamesInCate(int IdCateg);
        public Task<IEnumerable<Game>> FindVideoGames(string Keyword);
        public Task<bool> CreateVideoGame(Game videoGame);
        public Task<bool> UpdateVideoGame(Game videoGame);
        public Task<bool> DeleteVideoGame(Game videoGame);
        public Task<bool> ExistVideoGame(int id);
        public Task<bool> ExistVideoGame(string Title);
        public Task<bool> Save();

    }
}
