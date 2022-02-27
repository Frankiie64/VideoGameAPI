using PVideoGamesAPI.Models.Tables_Complements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagenRoute { get; set; }
        public string Sumary { get; set; }
        public string Duration { get; set; }
        public DateTime Release_Date { get; set; }
        public enum Clasificacion { A, B, B15, C, D }
        public Clasificacion clasificacion { get; set; }

        public string Developers { get; set; }
        public string Platforms { get; set; }

        public int IdCategory { get; set; }
        [ForeignKey("IdCategory")]
        public Category Category { get; set; }

        public int IdRequirements { get; set; }
        [ForeignKey("IdRequirements")]

        public Requeriments requirements { get; set; }


    }
}
