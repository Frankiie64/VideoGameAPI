using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PVideoGamesAPI.Models.Tables_Complements;
using PVideoGamesAPI.Models.Tables_Complements.Complements_Dtos;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Controllers
{
    [Authorize]
    [Route("api/Requirements")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APIRequeriments")]    
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class RequirementsController : Controller
    {
        private readonly IRepositoryRequirements _repo;
        private readonly IMapper _mapper;
        public RequirementsController(IRepositoryRequirements repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
   

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<RequirementsDto>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetRequirements()
        {
            List<Requeriments> list = await _repo.GetRequirements();
            List<RequirementsDto> listDto = new List<RequirementsDto>();

            foreach (var item in list)
            {
                listDto.Add(_mapper.Map<RequirementsDto>(item));
            }

            return Ok(listDto);
        }

        [AllowAnonymous]
        [HttpGet("{IdRequirements:int}", Name = "GetRequirements")]
        [ProducesResponseType(200, Type = typeof(List<RequirementsDto>))]
        [ProducesResponseType(404)]

        public async Task<ActionResult> GetRequirements(int IdRequirements)
        {
            Requeriments item = await _repo.GetRequiremnt(IdRequirements);

            if (item == null)
            {
                return NotFound();
            }

            RequirementsDto requirementsDto = _mapper.Map<RequirementsDto>(item);

            return Ok(requirementsDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<RequirementsCreateDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CreateRequirements([FromBody] RequirementsDto requirementsDto)
        {
            if (requirementsDto == null)
            {
                return BadRequest(ModelState);
            }

            bool value = await _repo.ExistRequiremnts(requirementsDto.Os);

            if (value)
            {
                ModelState.AddModelError("", "Los requeimientos ya existen para esa entrega.");
                return StatusCode(404, ModelState);
            }

            Requeriments item = _mapper.Map<Requeriments>(requirementsDto);

            value = await _repo.CreateRequiremnts(item);

            if (!value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetRequirements", new { IdRequirements = item.Id }, item);
        }
        
        [HttpPatch("{IdRequirements:int}", Name = "UpdateRequirements")]
        [ProducesResponseType(204, Type = typeof(List<RequirementsDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateRequirements([FromBody] RequirementsDto requirementsDto, int IdRequirements)
        {
            if (requirementsDto.Id != IdRequirements)
            {
                ModelState.AddModelError("", "Los id son diferente.");
                return StatusCode(400, ModelState);
            }
            if (requirementsDto == null)
            {
                ModelState.AddModelError("", "El formulario esta vacio");
                return StatusCode(400, ModelState);
            }
            bool Value = await _repo.ExistRequiremnts(IdRequirements);

            if (!Value)
            {
                ModelState.AddModelError("", "La requiremientos que desea actualizar, no existe en la base de datos.");
                return StatusCode(404, ModelState);
            }

            Requeriments requirements = _mapper.Map<Requeriments>(requirementsDto);
            Value = await _repo.UpdateRequiremnts(requirements);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetRequirements", new { IdRequirements = requirements.Id }, requirements);
        }

        [HttpDelete("{IdRequirements:int}", Name = "DeleteRequirements")]
        [ProducesResponseType(200, Type = typeof(List<RequirementsDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteRequirements(int IdRequirements)
        {
            bool Value = await _repo.ExistRequiremnts(IdRequirements);

            if (!Value)
            {
                ModelState.AddModelError("", "La requerimientos que deseas eliminar no existen.");
                return StatusCode(404, ModelState);
            }

            Requeriments requirements = await _repo.GetRequiremnt(IdRequirements);

            Value = await _repo.DeleteRequiremnts(requirements);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return Ok("Se ha eliminado correctamente los requerimientos de la base de datos.");
        }
    }
}

