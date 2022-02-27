using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Models.Tables_Complements.Complements_Dtos
{
    public class RequirementsDto
    {        
        public int Id { get; set; }
        public string Os { get; set; }
        [Required(ErrorMessage = "Es necesario conocer el procesador que como minimo se puede utilizar.")]

        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Graphics { get; set; }
        public string DirectX { get; set; }
        [Required(ErrorMessage = "Es necesario saber cuanto de internet se necesita para usar las operaciones fuera del modo local.")]

        public string Network { get; set; }
        [Required(ErrorMessage = "Es necesario saber el espacio de almacenamiento que ocupa el video juego.")]

        public string Storage { get; set; }        
    }
}
