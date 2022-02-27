using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PVideoGamesAPI.Models;
using PVideoGamesAPI.Models.Dtos;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.Controllers
{
    [Authorize]
    [Route("api/Categories")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APICategory")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class CategoriesController : Controller
    {
        private readonly IRepositoryCategory _repo;
        private readonly IMapper _mapper;

        public CategoriesController(IRepositoryCategory repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        /// <summary>
        /// Devuelve todas las categirias que existen en la base de datos.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetCategories()
        {
            List<Category> list = await _repo.GetCategories();
            List<CategoryDto> listDto = new List<CategoryDto>();

            foreach (var item in list)
            {
                listDto.Add(_mapper.Map<CategoryDto>(item));
            }

            return Ok(listDto);
        }
        /// <summary>
        /// Devuelve una categoria en especifico mediante el Id que es solicitado en los parametros.
        /// </summary>
        /// <param name="IdCategory"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{IdCategory:int}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(404)]

        public async Task<ActionResult> GetCategory(int IdCategory)
        {
            Category item = await _repo.GetCategory(IdCategory);

            if (item == null)
            {
                return NotFound();
            }

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(item);

            return Ok(categoryDto);
        }
        /// <summary>
        /// Permite crear una categoria en la base de datos.
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(ModelState);
            }

            bool value = await _repo.ExistCategory(categoryDto.Name);

            if (value)
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            Category item = _mapper.Map<Category>(categoryDto);

            value = await _repo.CreateCategory(item);

            if (!value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { IdCategory = item.Id }, item);
        }
        /// <summary>
        /// Este nos permite editar una categoria mediante el id solicitado.
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <param name="IdCategory"></param>
        /// <returns></returns>
        [HttpPatch("{IdCategory:int}", Name = "UpdateCategory")]
        [ProducesResponseType(204, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateCategory([FromBody] CategoryDto categoryDto, int IdCategory)
        {
            if(categoryDto.Id != IdCategory)
            {
                ModelState.AddModelError("", "Los id son diferente.");
                return StatusCode(400, ModelState);
            }
            if(categoryDto == null)
            {
                ModelState.AddModelError("", "El formulario esta vacio");
                return StatusCode(400, ModelState);
            }
            bool Value = await _repo.ExistCategory(IdCategory);

            if (!Value)
            {
                ModelState.AddModelError("", "La categoria que desea actualizar, no existe en la base de datos.");
                return StatusCode(404, ModelState);
            }

            Category category = _mapper.Map<Category>(categoryDto);
            Value = await _repo.UpdateCategory(category);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { IdCategory = category.Id }, category);
        }
        /// <summary>
        /// Este permite eliminar una categoria, con el id que se pide en los parametros.
        /// </summary>
        /// <param name="IdCategory"></param>
        /// <returns></returns>
        [HttpDelete("{IdCategory:int}", Name = "DeleteCategory")]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteCategory(int IdCategory)
        {
            bool Value = await _repo.ExistCategory(IdCategory);

            if (!Value)
            {
                ModelState.AddModelError("", "La categoria que deseas eliminar no existe.");
                return StatusCode(404, ModelState);
            }

            Category category = await _repo.GetCategory(IdCategory);

            Value = await _repo.DeleteCategory(category);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return Ok($"Se ha eliminado correctamente la categoria {category.Name} de la base de datos.");
        }



    }
}
