using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Application.Models;
using WarehouseAPI.Application.Interfaces;

namespace WarehouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }


        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"No se encontró la categoría con id = {id}");
            }

            // Si la categoría se encontró, retornamos 200 OK con la entidad
            return Ok(category);
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            //return Ok($"Id: {category.Name}, Name: {category.Name}, Description: {category.Description}, ParentId: {category.ParentId}");

            /*
            if(string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest("Name is required.");
            }

            */

            try
            {
                if (!(category.ParentId == null || (int.TryParse(category.ParentId.ToString(), out int numero) && numero >= 0)))
                {
                    return BadRequest("ParentId not valid.");
                }

                var created = await _categoryService.CreateAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                // Para nombres duplicados o parent no existente
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // Para nombre vacío, etc.
                return BadRequest(new { error = ex.Message });
            }

            //var created = await _categoryService.CreateAsync(category);
            //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

            //await Task.Delay(100);
            //return Ok("Task finished.");
        }
    }
}
