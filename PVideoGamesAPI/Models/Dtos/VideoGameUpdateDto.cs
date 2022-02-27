using Microsoft.AspNetCore.Http;
using PVideoGamesAPI.Models.Tables_Complements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PVideoGamesAPI.Models.Game;

namespace PVideoGamesAPI.Models.Dtos
{
    public class VideoGameUpdateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Es necesario que la entrega tenga un titulo.")]
        public string Title { get; set; }
        public string Sumary { get; set; }
        public string ImagenRoute { get; set; }
        public IFormFile Photo { get; set; }
        public string Duration { get; set; }
        public DateTime Release_Date { get; set; }
        [Required(ErrorMessage = "Es necesario que la entrega tenga una clasificacion del publico que que va dirigido.")]

        public Clasificacion clasificacion { get; set; }
        public string Developers { get; set; }
        public string Platforms { get; set; }
        [Required(ErrorMessage = "Es necesario que la entrega tenga una categoria.")]

        public int IdCategory { get; set; }
        public int IdRequirements { get; set; }

    }
}
