using Recipes_Api.Repositories;
using Recipes_Api.Models;

namespace Recipes_Api.Services
{
    public class RecipeService
    {
        private readonly IRecipeRepository _repository;

        public RecipeService(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Recipe> GetAll() => _repository.GetAll();

        public Recipe? GetByTitle(string title) => _repository.GetByTitle(title);

        public Recipe? GetById(int id) => _repository.GetById(id);
        public void Add(Recipe recipe) => _repository.Add(recipe);
        public bool Update(Recipe recipe) => _repository.Update(recipe);
        public bool Delete(int id) => _repository.Delete(id);

    }
}


