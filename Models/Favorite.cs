namespace Recipes_Api.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string? UserName { get; set; }
    }
}

