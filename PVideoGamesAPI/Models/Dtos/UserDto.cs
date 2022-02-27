using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string password { get; set; }

    }
}
