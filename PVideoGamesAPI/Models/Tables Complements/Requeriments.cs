using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Models.Tables_Complements
{
    public class Requeriments
    {
        [Key]
        public int Id { get; set; }
        public string Os { get; set; }
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Graphics { get; set; }
        public string DirectX { get; set; }
        public string Network { get; set; }
        public string Storage { get; set; }
    
    }
}
