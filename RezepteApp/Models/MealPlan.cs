using SQLite;

namespace RezepteApp.Models;

[Table("meal_plans")]
public class MealPlan
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public DateTime Date { get; set; }

    public string MealType { get; set; } = "Lunch"; // Breakfast, Lunch, Dinner

    public int Servings { get; set; } = 4;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Ignore]
    public Recipe? Recipe { get; set; }

    [Ignore]
    public string DateDisplay => Date.ToString("dddd, dd.MM.yyyy");
}

public static class MealType
{
    public const string Breakfast = "Frühstück";
    public const string Lunch = "Mittagessen";
    public const string Dinner = "Abendessen";

    public static List<string> AllTypes => new()
    {
        Breakfast,
        Lunch,
        Dinner
    };
}
