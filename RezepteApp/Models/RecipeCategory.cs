namespace RezepteApp.Models;

public static class RecipeCategory
{
    public const string Breakfast = "Frühstück";
    public const string Lunch = "Mittagessen";
    public const string Dinner = "Abendessen";
    public const string Dessert = "Dessert";
    public const string Snacks = "Snacks";
    public const string Drinks = "Getränke";

    public static List<string> AllCategories => new()
    {
        Breakfast,
        Lunch,
        Dinner,
        Dessert,
        Snacks,
        Drinks
    };
}

public static class Difficulty
{
    public const string Easy = "Einfach";
    public const string Medium = "Mittel";
    public const string Hard = "Schwer";

    public static List<string> AllLevels => new()
    {
        Easy,
        Medium,
        Hard
    };
}
