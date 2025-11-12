using System.Collections.Generic;
using Recipes_Api.Models;

namespace Recipes_Api.Repositories
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAll();
        Recipe? GetByTitle(string title);

        Recipe? GetById(int id);
        void Add(Recipe recipe);
        bool Update(Recipe recipe);
        bool Delete(int id);

        IEnumerable<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        void AddCategory(Category category);

        IEnumerable<Favorite> GetFavorites(string userName);
        void AddFavorite(Favorite favorite);
        bool RemoveFavorite(int id);



    }
}
