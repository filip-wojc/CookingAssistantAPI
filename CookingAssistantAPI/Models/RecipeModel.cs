using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;

namespace CookingAssistantAPI.Models
{
    public class RecipeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        [JsonProperty("rattings")]
        public int Ratings { get; set; }
        public string[] Ingredients { get; set; }
        public string[] Steps { get; set; }
        public Dictionary<string, string> Nutrients { get; set; }
        public Dictionary<string, string> Times { get; set; }
        public string Difficulty { get; set; }
        public string Subcategory { get; set; }
        [JsonProperty("dish_type")]
        public string DishType { get; set; }

        public RecipeModel() 
        { 
            Nutrients = new Dictionary<string, string>();
            Times = new Dictionary<string, string>();
        }

        public void Print()
        {
            Console.WriteLine("- - - - - - - - - - - - - - - - - -");
            Console.WriteLine("Id: " + Id);
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Description: " + Description);
            Console.WriteLine("Image: " + Image);
            Console.WriteLine("Author: " + Author);
            Console.WriteLine("Ratings: " + Ratings);
            Console.WriteLine("Ingredients: ");
            foreach (string ingredient in Ingredients)
            {
                Console.WriteLine(ingredient);
            }
            Console.WriteLine("Steps: ");
            foreach (string step in Steps)
            {
                Console.WriteLine(step);
            }
            Console.WriteLine("Nutrients: ");
            foreach (KeyValuePair<string, string> nutrient in Nutrients)
            {
                Console.WriteLine(nutrient.Key + ": " + nutrient.Value);
            }
            Console.WriteLine("Times: ");
            foreach (KeyValuePair<string, string> time in Times)
            {
                Console.WriteLine(time.Key + ": " + time.Value);
            }
            Console.WriteLine("Difficulty: " + Difficulty);
            Console.WriteLine("Subcategory: " + Subcategory);
            Console.WriteLine("DishType: " + DishType);
            Console.WriteLine("- - - - - - - - - - - - - - - - - -");
        }

    }
}
