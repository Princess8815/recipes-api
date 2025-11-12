using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Recipes_Api.Models;
using Microsoft.Extensions.Configuration;

namespace Recipes_Api.Repositories
{
    public class SqlRecipeRepository : IRecipeRepository
    {
        private readonly string _connectionString;

        public SqlRecipeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // ============================
        // 🍽️ RECIPE CRUD
        // ============================

        // ✅ GET ALL
        public IEnumerable<Recipe> GetAll()
        {
            var recipes = new List<Recipe>();

            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, ImageUrl, Ingredients, Instructions, CookingTime, CategoryId FROM Recipes";
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                recipes.Add(new Recipe
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    ImageUrl = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Ingredients = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Instructions = reader.IsDBNull(4) ? null : reader.GetString(4),
                    CookingTime = reader.IsDBNull(5) ? null : reader.GetString(5),
                    CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                });
            }

            return recipes;
        }

        // ✅ GET BY ID
        public Recipe? GetById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, ImageUrl, Ingredients, Instructions, CookingTime, CategoryId FROM Recipes WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Recipe
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    ImageUrl = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Ingredients = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Instructions = reader.IsDBNull(4) ? null : reader.GetString(4),
                    CookingTime = reader.IsDBNull(5) ? null : reader.GetString(5),
                    CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                };
            }

            return null;
        }

        // ✅ GET BY TITLE
        public Recipe? GetByTitle(string title)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, ImageUrl, Ingredients, Instructions, CookingTime, CategoryId FROM Recipes WHERE Title = @title";
            cmd.Parameters.AddWithValue("@title", title);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Recipe
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    ImageUrl = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Ingredients = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Instructions = reader.IsDBNull(4) ? null : reader.GetString(4),
                    CookingTime = reader.IsDBNull(5) ? null : reader.GetString(5),
                    CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                };
            }

            return null;
        }

        // ✅ ADD (POST)
        public void Add(Recipe recipe)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Recipes (Title, ImageUrl, Ingredients, Instructions, CookingTime, CategoryId)
                VALUES (@title, @imageUrl, @ingredients, @instructions, @cookingTime, @categoryId)";
            cmd.Parameters.AddWithValue("@title", recipe.Title);
            cmd.Parameters.AddWithValue("@imageUrl", recipe.ImageUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ingredients", recipe.Ingredients ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@instructions", recipe.Instructions ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@cookingTime", recipe.CookingTime ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@categoryId", recipe.CategoryId ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        // ✅ UPDATE (PUT)
        public bool Update(Recipe updatedRecipe)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Recipes
                SET Title = @title,
                    ImageUrl = @imageUrl,
                    Ingredients = @ingredients,
                    Instructions = @instructions,
                    CookingTime = @cookingTime,
                    CategoryId = @categoryId
                WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", updatedRecipe.Id);
            cmd.Parameters.AddWithValue("@title", updatedRecipe.Title);
            cmd.Parameters.AddWithValue("@imageUrl", updatedRecipe.ImageUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ingredients", updatedRecipe.Ingredients ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@instructions", updatedRecipe.Instructions ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@cookingTime", updatedRecipe.CookingTime ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@categoryId", updatedRecipe.CategoryId ?? (object)DBNull.Value);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        // ✅ DELETE
        public bool Delete(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Recipes WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        // ============================
        // 🏷️ CATEGORY CRUD
        // ============================

        public IEnumerable<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name FROM Categories";
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return categories;
        }

        public Category? GetCategoryById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name FROM Categories WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
            }

            return null;
        }

        public void AddCategory(Category category)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Categories (Name) VALUES (@name)";
            cmd.Parameters.AddWithValue("@name", category.Name);
            cmd.ExecuteNonQuery();
        }

        public bool UpdateCategory(Category category)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Categories SET Name = @name WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", category.Id);
            cmd.Parameters.AddWithValue("@name", category.Name);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool DeleteCategory(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Categories WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        // ============================
        // 💖 FAVORITES CRUD
        // ============================

        public IEnumerable<Favorite> GetFavorites(string userName)
        {
            var favorites = new List<Favorite>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, RecipeId, UserName FROM Favorites WHERE UserName = @user";
            cmd.Parameters.AddWithValue("@user", userName);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                favorites.Add(new Favorite
                {
                    Id = reader.GetInt32(0),
                    RecipeId = reader.GetInt32(1),
                    UserName = reader.IsDBNull(2) ? null : reader.GetString(2)
                });
            }

            return favorites;
        }

        public Favorite? GetFavoriteById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, RecipeId, UserName FROM Favorites WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Favorite
                {
                    Id = reader.GetInt32(0),
                    RecipeId = reader.GetInt32(1),
                    UserName = reader.IsDBNull(2) ? null : reader.GetString(2)
                };
            }
            return null;
        }

        public void AddFavorite(Favorite favorite)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Favorites (RecipeId, UserName) VALUES (@recipeId, @user)";
            cmd.Parameters.AddWithValue("@recipeId", favorite.RecipeId);
            cmd.Parameters.AddWithValue("@user", favorite.UserName ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public bool UpdateFavorite(Favorite favorite)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Favorites SET RecipeId = @recipeId, UserName = @user WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", favorite.Id);
            cmd.Parameters.AddWithValue("@recipeId", favorite.RecipeId);
            cmd.Parameters.AddWithValue("@user", favorite.UserName ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
            return true;
        }

        public bool RemoveFavorite(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Favorites WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
    }
}

