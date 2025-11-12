using Microsoft.AspNetCore.Mvc;
using Recipes_Api.Models;
using Recipes_Api.Services;

namespace Recipes_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly RecipeService _service;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(RecipeService service, ILogger<RecipesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ✅ GET: api/recipes
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var recipes = _service.GetAll();
                if (recipes == null || !recipes.Any())
                    return NotFound("No recipes found.");
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all recipes");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // ✅ GET: api/recipes/{title}
        [HttpGet("{title}")]
        public IActionResult GetByTitle(string title)
        {
            try
            {
                var recipe = _service.GetByTitle(title);
                if (recipe == null)
                    return NotFound($"Recipe '{title}' not found.");

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipe by title");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // ✅ GET: api/recipes/id/{id}
        [HttpGet("id/{id}")]
        public IActionResult GetRecipeById(int id)
        {
            try
            {
                var recipe = _service.GetById(id);
                if (recipe == null)
                    return NotFound($"Recipe with ID {id} not found.");

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recipe by ID");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // ✅ POST: api/recipes
        [HttpPost]
        public IActionResult CreateRecipe([FromBody] Recipe recipe)
        {
            try
            {
                if (recipe == null)
                    return BadRequest("Recipe data is required.");

                _service.Add(recipe);
                _logger.LogInformation("Recipe '{Title}' created successfully.", recipe.Title);

                return CreatedAtAction(nameof(GetRecipeById), new { id = recipe.Id }, recipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating recipe");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // ✅ PUT: api/recipes/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateRecipe(int id, [FromBody] Recipe updatedRecipe)
        {
            try
            {
                if (updatedRecipe == null || updatedRecipe.Id != id)
                    return BadRequest("Invalid recipe data.");

                var success = _service.Update(updatedRecipe);
                if (!success)
                    return NotFound($"Recipe with ID {id} not found.");

                _logger.LogInformation("Recipe '{Title}' updated successfully.", updatedRecipe.Title);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // ✅ DELETE: api/recipes/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            try
            {
                var success = _service.Delete(id);
                if (!success)
                    return NotFound($"Recipe with ID {id} not found.");

                _logger.LogInformation("Recipe with ID {Id} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}



