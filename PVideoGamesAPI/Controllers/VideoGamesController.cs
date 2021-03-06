using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PVideoGamesAPI.Models;
using PVideoGamesAPI.Models.Dtos;
using PVideoGamesAPI.Models.Tables_Complements;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Controllers
{
    [Authorize]
    [Route("api/VideoGames")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APIVideoGame")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class VideoGamesController : Controller
    {
        private readonly IRepositoryVideoGame _repo;
        private readonly IWebHostEnvironment _Hosting;
        private readonly IMapper _mapper;

        public VideoGamesController(IRepositoryVideoGame repo, IMapper mapper, IWebHostEnvironment Hosting)
        {
            _repo = repo;
            _Hosting = Hosting;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<VideoGameDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetVideoGames()
        {
            List<Game> list = await _repo.GetVideoGames();

            List<VideoGameDto> listDto = new List<VideoGameDto>();

            foreach (var item in list)
            {
                listDto.Add(_mapper.Map<VideoGameDto>(item));

            }

            return Ok(listDto);

        }     
        [AllowAnonymous]
        [HttpGet("{IdVideoGame:int}", Name = "GetVideoGame")]
        [ProducesResponseType(200, Type = typeof(List<VideoGameDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public async Task<ActionResult> GetVideoGame(int IdVideoGame)
        {
            Game item = await _repo.GetVideoGame(IdVideoGame);

            if (item == null)
            {
                return NotFound();
            }

            VideoGameDto videogameDto = _mapper.Map<VideoGameDto>(item);

            return Ok(videogameDto);
        }
        [AllowAnonymous]
        [HttpGet("GetVideoGameInCate/{IdCategoria:int}")]
        [ProducesResponseType(200, Type = typeof(List<VideoGameDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public async Task<ActionResult> GetVideoGameInCate(int IdCategoria)
        {
            List<Game> ListvideoGame = await _repo.GetVideoGamesInCate(IdCategoria);
            List<VideoGameDto> ListvideoGameDto = new List<VideoGameDto>();

            if (ListvideoGame == null)
            {
                return NotFound();
            }

            foreach (Game item in ListvideoGame)
            {
               ListvideoGameDto.Add(_mapper.Map<VideoGameDto>(item));
            }

            return Ok(ListvideoGameDto);
        }
        [AllowAnonymous]
        [HttpGet("FindVideoGames")]
        [ProducesResponseType(200, Type = typeof(List<VideoGameDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> FindVideoGames(string keyword)
        {
            try
            {
                var result = await _repo.FindVideoGames(keyword);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception Ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");
            }
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<VideoGameCreateDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CreateVideoGame([FromForm] VideoGameCreateDto videogameDto)
        {
            try
            {
                if (videogameDto == null)
                {
                    return BadRequest(ModelState);
                }

                bool value = await _repo.ExistVideoGame(videogameDto.Title);

                if (value)
                {
                    ModelState.AddModelError("", "El video juego ya fue creado.");
                    return StatusCode(404, ModelState);
                }

                var File = videogameDto.Photo;
                string PrincipalRoute = _Hosting.WebRootPath;
                var Files = HttpContext.Request.Form.Files;


                if (File.Length > 0)
                {
                    var PhotoName = Guid.NewGuid().ToString();
                    var Uploads = Path.Combine(PrincipalRoute, @"Photos");
                    var Extension = Path.GetExtension(Files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(Uploads, PhotoName + Extension), FileMode.Create))
                    {
                        Files[0].CopyTo(fileStreams);
                    }

                    videogameDto.ImagenRoute = @"\Photos\" + PhotoName + Extension;
                }


                Game item = _mapper.Map<Game>(videogameDto);

                value = await _repo.CreateVideoGame(item);

                if (!value)
                {
                    ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtRoute("GetVideoGame", new { IdVideoGame = item.Id }, item);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", $"Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico, {ex}");
                return StatusCode(500, ModelState);
            }
        }        
        [HttpPatch("{IdVideoGame:int}", Name = "UpdateVideoGame")]
        [ProducesResponseType(204, Type = typeof(List<VideoGameUpdateDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateVideoGame([FromForm] VideoGameUpdateDto videogameDto, int IdVideoGame)
        {
            if (videogameDto.Id != IdVideoGame)
            {
                ModelState.AddModelError("", "Los id son diferente.");
                return StatusCode(400, ModelState);
            }
            if (videogameDto == null)
            {
                ModelState.AddModelError("", "El formulario esta vacio");
                return StatusCode(400, ModelState);
            }
            bool Value = await _repo.ExistVideoGame(IdVideoGame);

            if (!Value)
            {
                ModelState.AddModelError("", "El video juego que desea actualizar, no existe en la base de datos.");
                return StatusCode(404, ModelState);
            }
            var File = videogameDto.Photo;
            string PrincipalRoute = _Hosting.WebRootPath;
            var Files = HttpContext.Request.Form.Files;


            if (File.Length > 0)
            {
                var PhotoName = Guid.NewGuid().ToString();
                var Uploads = Path.Combine(PrincipalRoute, @"Photos");
                var Extension = Path.GetExtension(Files[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(Uploads, PhotoName + Extension), FileMode.Create))
                {
                    Files[0].CopyTo(fileStreams);
                }

                videogameDto.ImagenRoute = @"\Photos\" + PhotoName + Extension;
            }

            Game videogame = _mapper.Map<Game>(videogameDto);
            Value = await _repo.UpdateVideoGame(videogame);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetVideoGame", new { IdVideoGame = videogame.Id }, videogame);
        }
        [HttpDelete("{IdVideoGame:int}", Name = "DeleteVideoGame")]
        [ProducesResponseType(200, Type = typeof(List<VideoGameDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteVideoGame(int IdVideoGame)
        {
            bool Value = await _repo.ExistVideoGame(IdVideoGame);

            if (!Value)
            {
                ModelState.AddModelError("", "El video juego que deseas eliminar no existe.");
                return StatusCode(404, ModelState);
            }

            Game videogame = await _repo.GetVideoGame(IdVideoGame);

            Value = await _repo.DeleteVideoGame(videogame);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return Ok($"Se ha eliminado correctamente la entrega de  {videogame.Title} de la base de datos.");
        }


    }
}
