using Microsoft.AspNetCore.Mvc;
using Recipes_Api.Repositories;
using Recipes_Api.Models;

namespace Recipes_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRecipeRepository _repository;

        public CategoriesController(IRecipeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.GetAllCategories());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _repository.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Category category)
        {
            _repository.AddCategory(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Category category)
        {
            var existing = _repository.GetCategoryById(category.Id);
            if (existing == null) return NotFound();
            existing.Name = category.Name;
            // Assuming there's an UpdateCategory method in the repository
            _repository.UpdateCategory(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _repository.GetCategoryById(id);
            if (existing == null) return NotFound();
            // Assuming there's a DeleteCategory method in the repository
            _repository.DeleteCategory(id);
            return NoContent();
        }
    }
}
