using System.Collections.Generic;

namespace Recipes_Api.Models
{
    public class RecipeDetails
    {
        public string? Description { get; set; }
        public List<string>? Ingredients { get; set; }
        public string? Instructions { get; set; }
    }
}

