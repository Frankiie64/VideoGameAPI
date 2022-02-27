using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PVideoGamesAPI.Models;
using PVideoGamesAPI.Models.Dtos;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Controllers
{
    [Authorize]
    [Route("api/User")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APIUsers")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class UsersController : Controller
    {
        private readonly IRepositoryUser _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(IRepositoryUser repo, IMapper mapper, IConfiguration config)
        {
            _repo = repo;
            _mapper = mapper;
            _config = config;
        }
        /// <summary>
        /// Devuelve todos los usario que hay en la base de datos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetUsers()
        {
            List<User> list = await _repo.GetUsers();
            List<UserDto> listDto = new List<UserDto>();

            foreach (var item in list)
            {
                listDto.Add(_mapper.Map<UserDto>(item));
            }

            return Ok(listDto);
        }
        /// <summary>
        /// Devuelve en especifico un usuario mediante el Id.
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        [HttpGet("{IdUser:int}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        [ProducesResponseType(404)]

        public async Task<ActionResult> GetUser(int IdUser)
        {
            User item = await _repo.GetUser(IdUser);

            if (item == null)
            {
                return NotFound();
            }

            UserDto userDto = _mapper.Map<UserDto>(item);

            return Ok(userDto);
        }
        /// <summary>
        /// Se necesita tanto el nombre del usuario como la contraseña para devolver el token de entrada a la app.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(202, Type = typeof(List<UserLoginDto>))]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public async Task<IActionResult> Login([FromForm] UserLoginDto userDto)
        {
            User item = await _repo.Login(userDto.Password, userDto.Nickname);

            if (item == null)
            {
                return Unauthorized("Los datos no coinciden con los dato que se han  encontrado en la base de datos.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,item.Id.ToString()),
                new Claim(ClaimTypes.Name,item.Nickname.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = Credentials
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.CreateToken(TokenDescriptor);

            return Ok(new
            {
                token = TokenHandler.WriteToken(token)
            });
        }
        /// <summary>
        /// Permite registrar a nuevos usuarios en la base de datos.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Registro")]
        [ProducesResponseType(201, Type = typeof(List<UserRegisterDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> RegisterUser([FromForm] UserRegisterDto userDto)
        {
            userDto.Nickname = userDto.Nickname.ToLower();

            var Value = await _repo.ExistUser(userDto.Nickname);

            if (Value)
            {
                BadRequest("El usuario que desea registrar ya existe.");
            }

            User NewUser = new User
            {
                Nickname = userDto.Nickname
            };

            var user = await _repo.Register(NewUser,userDto.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }
            return Ok(userDto);
        }
        /// <summary>
        /// Este nos permite editar los usuarios creados.
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        [HttpPatch("{IdUser:int}", Name = "UpdateUser")]
        [ProducesResponseType(204, Type = typeof(List<UserDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateUser([FromBody] UserDto userDto, int IdUser)
        {
            if (userDto.Id != IdUser)
            {
                ModelState.AddModelError("", "Los id son diferente.");
                return StatusCode(400, ModelState);
            }
            if (userDto == null)
            {
                ModelState.AddModelError("", "El formulario esta vacio");
                return StatusCode(400, ModelState);
            }
            bool Value = await _repo.ExistUser(IdUser);

            if (!Value)
            {
                ModelState.AddModelError("", "El usuario que desea actualizar, no existe en la base de datos.");
                return StatusCode(404, ModelState);
            }

            User user = _mapper.Map<User>(userDto);
            Value = await _repo.UpdateUser(user,userDto.password);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetUser", new { IdUser = userDto.Id }, userDto);
        }
        /// <summary>
        /// Permite elimianar los usarios mediante el Id requerido en los parametros.
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        [HttpDelete("{IdUser:int}", Name = "DeleteUser")]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteUser(int IdUser)
        {
            bool Value = await _repo.ExistUser(IdUser);

            if (!Value)
            {
                ModelState.AddModelError("", "La categoria que deseas eliminar no existe.");
                return StatusCode(404, ModelState);
            }

            User user = await _repo.GetUser(IdUser);

            Value = await _repo.DeleteUser(user);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return Ok($"Se ha eliminado correctamente el usuario {user.Nickname} de la base de datos.");
        }

    }
}
