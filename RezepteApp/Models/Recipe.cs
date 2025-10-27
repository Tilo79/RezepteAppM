using SQLite;

namespace RezepteApp.Models;

[Table("recipes")]
public class Recipe
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(200), NotNull]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public string Ingredients { get; set; } = string.Empty; // JSON string

    public string Instructions { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Difficulty { get; set; } = "Medium";

    public int CookingTimeMinutes { get; set; }

    public int PrepTimeMinutes { get; set; }

    public int Servings { get; set; } = 4;

    public bool IsFavorite { get; set; }

    public double AverageRating { get; set; }

    public int RatingCount { get; set; }

    // Nutritional Information (per serving)
    public int Calories { get; set; }
    public double Protein { get; set; }
    public double Carbohydrates { get; set; }
    public double Fat { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Ignore]
    public List<Ingredient> IngredientList
    {
        get => string.IsNullOrEmpty(Ingredients) 
            ? new List<Ingredient>() 
            : System.Text.Json.JsonSerializer.Deserialize<List<Ingredient>>(Ingredients) ?? new List<Ingredient>();
        set => Ingredients = System.Text.Json.JsonSerializer.Serialize(value);
    }

    [Ignore]
    public string TotalTime => $"{PrepTimeMinutes + CookingTimeMinutes} Min";

    [Ignore]
    public string RatingDisplay => AverageRating > 0 ? $"⭐ {AverageRating:F1}" : "Noch nicht bewertet";
}

public class Ingredient
{
    public string Name { get; set; } = string.Empty;
    public double Amount { get; set; }
    public string Unit { get; set; } = string.Empty;

    public override string ToString() => $"{Amount} {Unit} {Name}";
}
