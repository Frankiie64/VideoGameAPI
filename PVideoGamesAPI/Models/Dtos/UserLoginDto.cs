using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Models.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Nickname { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatorio.")]
        public string Password { get; set; }
    }
}

