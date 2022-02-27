using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Models.Dtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Nickname { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatorio.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Tiene que ingresar una contraseña que estar entre 4 y 10 caracteres.")]
        public string Password { get; set; }
    }
}
