using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Recipes_Api.Models;

namespace Recipes_Api.Repositories
{
    public class JsonRecipeRepository : IRecipeRepository
    {
        private readonly string _recipesPath;
        private readonly string _categoriesPath;
        private readonly string _favoritesPath;

        private readonly List<Recipe> _recipes;
        private readonly List<Category> _categories;
        private readonly List<Favorite> _favorites;

        public JsonRecipeRepository()
        {
            var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dataDir);

            _recipesPath = Path.Combine(dataDir, "recipe.json");
            _categoriesPath = Path.Combine(dataDir, "categories.json");
            _favoritesPath = Path.Combine(dataDir, "favorites.json");

            _recipes = Load<List<Recipe>>(_recipesPath) ?? new List<Recipe>();
            _categories = Load<List<Category>>(_categoriesPath) ?? new List<Category>();
            _favorites = Load<List<Favorite>>(_favoritesPath) ?? new List<Favorite>();
        }

        // ============================
        // 🍽️ RECIPE CRUD
        // ============================

        public IEnumerable<Recipe> GetAll() => _recipes;

        public Recipe? GetById(int id) => _recipes.FirstOrDefault(r => r.Id == id);

        public Recipe? GetByTitle(string title) =>
            _recipes.FirstOrDefault(r =>
                r.Title != null &&
                r.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));

        public void Add(Recipe recipe)
        {
            recipe.Id = _recipes.Any() ? _recipes.Max(r => r.Id) + 1 : 1;
            _recipes.Add(recipe);
            Save(_recipesPath, _recipes);
        }

        public bool Update(Recipe updatedRecipe)
        {
            var existing = _recipes.FirstOrDefault(r => r.Id == updatedRecipe.Id);
            if (existing == null)
                return false;

            existing.Title = updatedRecipe.Title;
            existing.ImageUrl = updatedRecipe.ImageUrl;
            existing.Ingredients = updatedRecipe.Ingredients;
            existing.Instructions = updatedRecipe.Instructions;
            existing.CookingTime = updatedRecipe.CookingTime;
            existing.CategoryId = updatedRecipe.CategoryId;

            Save(_recipesPath, _recipes);
            return true;
        }

        public bool Delete(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return false;

            _recipes.Remove(recipe);
            Save(_recipesPath, _recipes);
            return true;
        }

        // ============================
        // 🏷️ CATEGORY CRUD
        // ============================

        public IEnumerable<Category> GetAllCategories() => _categories;

        public Category? GetCategoryById(int id) =>
            _categories.FirstOrDefault(c => c.Id == id);

        public void AddCategory(Category category)
        {
            category.Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
            _categories.Add(category);
            Save(_categoriesPath, _categories);
        }

        public bool UpdateCategory(Category category)
        {
            var existing = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existing == null)
                return false;

            existing.Name = category.Name;
            Save(_categoriesPath, _categories);
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return false;

            _categories.Remove(category);
            Save(_categoriesPath, _categories);
            return true;
        }

        // ============================
        // 💖 FAVORITES CRUD
        // ============================

        public IEnumerable<Favorite> GetFavorites(string userName) =>
            _favorites.Where(f =>
                f.UserName != null &&
                f.UserName.Equals(userName, System.StringComparison.OrdinalIgnoreCase));

        public void AddFavorite(Favorite favorite)
        {
            favorite.Id = _favorites.Any() ? _favorites.Max(f => f.Id) + 1 : 1;
            _favorites.Add(favorite);
            Save(_favoritesPath, _favorites);
        }

        public bool RemoveFavorite(int id)
        {
            var favorite = _favorites.FirstOrDefault(f => f.Id == id);
            if (favorite == null)
                return false;

            _favorites.Remove(favorite);
            Save(_favoritesPath, _favorites);
            return true;
        }

        // ============================
        // 🔧 HELPER METHODS
        // ============================

        private static T? Load<T>(string path)
        {
            if (!File.Exists(path)) return default;
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private static void Save<T>(string path, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(path, json);
        }

        public Favorite? GetFavoriteById(int id)
        {
            return _favorites.FirstOrDefault(f => f.Id == id);
        }

        public bool UpdateFavorite(Favorite favorite)
        {
            var existing = _favorites.FirstOrDefault(f => f.Id == favorite.Id);
            if (existing == null)
                return false;
            existing.UserName = favorite.UserName;
            existing.RecipeId = favorite.RecipeId;
            Save(_favoritesPath, _favorites);
            return true;
        }
    }
}

