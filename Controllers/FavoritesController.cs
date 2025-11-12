using Microsoft.AspNetCore.Mvc;
using Recipes_Api.Repositories;
using Recipes_Api.Models;

namespace Recipes_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IRecipeRepository _repository;

        public FavoritesController(IRecipeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{userName}")]
        public IActionResult GetFavorites(string userName)
        {
            var list = _repository.GetFavorites(userName);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult AddFavorite([FromBody] Favorite favorite)
        {
            _repository.AddFavorite(favorite);
            return Ok(favorite);
        }

        [HttpPut]
        public IActionResult UpdateFavorite([FromBody] Favorite favorite)
        {
            var existing = _repository.GetFavoriteById(favorite.Id);
            if (existing == null) return NotFound();
            existing.UserName = favorite.UserName;
            existing.RecipeId = favorite.RecipeId;
            _repository.UpdateFavorite(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFavorite(int id)
        {
            if (!_repository.RemoveFavorite(id))
                return NotFound();
            return NoContent();
        }
    }
}

