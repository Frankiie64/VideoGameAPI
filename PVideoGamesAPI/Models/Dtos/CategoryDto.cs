using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Es necesario poner el nombre de la categoria.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Es necesario poner la descripcion de la categoria.")]
        public string Sumary { get; set; }

    }
}
