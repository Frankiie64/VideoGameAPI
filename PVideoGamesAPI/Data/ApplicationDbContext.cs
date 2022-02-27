using Microsoft.EntityFrameworkCore;
using PVideoGamesAPI.Models;
using PVideoGamesAPI.Models.Tables_Complements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PVideoGamesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base (options)
        {

        }

        public DbSet<Category> category { get; set; }
        public DbSet<Requeriments> requeriments { get; set; }

        public DbSet<Game> Game { get; set; }

        public DbSet<User> user { get; set; }
    }
}
