namespace Recipes_Api.Models
{
    public class Recipe
    {
        public int Id { get; set; }                // unique identifier
        public string Title { get; set; } = "";    // name of recipe
        public string? ImageUrl { get; set; }      // image link
        public string? Ingredients { get; set; }   // list of ingredients
        public string? Instructions { get; set; }  // step-by-step instructions
        public string? CookingTime { get; set; }       // minutes or time estimate

        public int? CategoryId { get; set; }

    }
}


