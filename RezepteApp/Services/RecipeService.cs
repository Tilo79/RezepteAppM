using RezepteApp.Data;
using RezepteApp.Models;

namespace RezepteApp.Services;

public class RecipeService
{
    private readonly RecipeDatabase _database;

    public RecipeService(RecipeDatabase database)
    {
        _database = database;
    }

    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return await _database.GetRecipesAsync();
    }

    public async Task<Recipe?> GetRecipeByIdAsync(int id)
    {
        return await _database.GetRecipeAsync(id);
    }

    public async Task<List<Recipe>> GetFavoriteRecipesAsync()
    {
        return await _database.GetFavoriteRecipesAsync();
    }

    public async Task<List<Recipe>> SearchRecipesAsync(string searchText, string? category = null, string? difficulty = null)
    {
        var recipes = string.IsNullOrWhiteSpace(searchText)
            ? await _database.GetRecipesAsync()
            : await _database.SearchRecipesAsync(searchText);

        if (!string.IsNullOrEmpty(category) && category != "Alle")
            recipes = recipes.Where(r => r.Category == category).ToList();

        if (!string.IsNullOrEmpty(difficulty) && difficulty != "Alle")
            recipes = recipes.Where(r => r.Difficulty == difficulty).ToList();

        return recipes;
    }

    public async Task<int> SaveRecipeAsync(Recipe recipe)
    {
        return await _database.SaveRecipeAsync(recipe);
    }

    public async Task<int> DeleteRecipeAsync(Recipe recipe)
    {
        return await _database.DeleteRecipeAsync(recipe);
    }

    public async Task<int> ToggleFavoriteAsync(Recipe recipe)
    {
        recipe.IsFavorite = !recipe.IsFavorite;
        return await _database.SaveRecipeAsync(recipe);
    }

    public async Task<int> RateRecipeAsync(Recipe recipe, int rating)
    {
        var totalRating = (recipe.AverageRating * recipe.RatingCount) + rating;
        recipe.RatingCount++;
        recipe.AverageRating = totalRating / recipe.RatingCount;
        return await _database.SaveRecipeAsync(recipe);
    }

    public async Task SeedDataAsync()
    {
        var recipes = await GetAllRecipesAsync();
        if (recipes.Any())
            return;

        var sampleRecipes = new List<Recipe>
        {
            new Recipe
            {
                Name = "Spaghetti Carbonara",
                Description = "Ein klassisches italienisches Pasta-Gericht mit Speck, Ei und Parmesan.",
                Category = RecipeCategory.Lunch,
                Difficulty = Difficulty.Medium,
                CookingTimeMinutes = 20,
                PrepTimeMinutes = 10,
                Servings = 4,
                Ingredients = @"[
                    ""400g Spaghetti"",
                    ""200g Speck (Guanciale oder Pancetta)"",
                    ""4 Eier"",
                    ""100g Parmesan (gerieben)"",
                    ""Salz und schwarzer Pfeffer""
                ]",
                Instructions = @"1. Spaghetti nach Packungsanleitung kochen.
2. Speck in kleine Würfel schneiden und in einer Pfanne knusprig braten.
3. Eier mit Parmesan verquirlen.
4. Gekochte Spaghetti abgießen und zum Speck geben.
5. Pfanne vom Herd nehmen und Ei-Mix unterheben.
6. Mit Salz und Pfeffer abschmecken.",
                Calories = 520,
                Protein = 22,
                Carbohydrates = 65,
                Fat = 18,
                AverageRating = 4.5,
                RatingCount = 12,
                IsFavorite = true
            },
            new Recipe
            {
                Name = "Caesar Salat",
                Description = "Knackiger Römersalat mit hausgemachtem Caesar Dressing und Croutons.",
                Category = RecipeCategory.Lunch,
                Difficulty = Difficulty.Easy,
                CookingTimeMinutes = 0,
                PrepTimeMinutes = 15,
                Servings = 2,
                Ingredients = @"[
                    ""1 Römersalat"",
                    ""100g Parmesan"",
                    ""2 Scheiben Weißbrot"",
                    ""4 EL Mayonnaise"",
                    ""2 EL Zitronensaft"",
                    ""1 Knoblauchzehe"",
                    ""Worcestershire-Sauce"",
                    ""Olivenöl""
                ]",
                Instructions = @"1. Römersalat waschen und in mundgerechte Stücke schneiden.
2. Für das Dressing: Mayonnaise, Parmesan, Zitronensaft, Knoblauch und Worcestershire-Sauce vermischen.
3. Brot in Würfel schneiden und in der Pfanne mit Olivenöl knusprig braten.
4. Salat mit Dressing vermengen, Croutons und Parmesan darüber geben.",
                Calories = 340,
                Protein = 12,
                Carbohydrates = 28,
                Fat = 22,
                AverageRating = 4.2,
                RatingCount = 8
            },
            new Recipe
            {
                Name = "Pancakes",
                Description = "Fluffige amerikanische Pfannkuchen zum Frühstück.",
                Category = RecipeCategory.Breakfast,
                Difficulty = Difficulty.Easy,
                CookingTimeMinutes = 15,
                PrepTimeMinutes = 10,
                Servings = 4,
                Ingredients = @"[
                    ""200g Mehl"",
                    ""2 EL Zucker"",
                    ""2 TL Backpulver"",
                    ""1 Prise Salz"",
                    ""250ml Milch"",
                    ""1 Ei"",
                    ""2 EL geschmolzene Butter"",
                    ""Ahornsirup und Beeren zum Servieren""
                ]",
                Instructions = @"1. Mehl, Zucker, Backpulver und Salz vermischen.
2. Milch, Ei und geschmolzene Butter unterrühren.
3. Pfanne erhitzen und etwas Butter zerlassen.
4. Teig portionsweise in die Pfanne geben.
5. Wenden, wenn Blasen aufsteigen.
6. Mit Ahornsirup und frischen Beeren servieren.",
                Calories = 280,
                Protein = 8,
                Carbohydrates = 42,
                Fat = 9,
                AverageRating = 4.8,
                RatingCount = 25,
                IsFavorite = true
            },
            new Recipe
            {
                Name = "Gemüse-Curry",
                Description = "Würziges vegetarisches Curry mit frischem Gemüse.",
                Category = RecipeCategory.Dinner,
                Difficulty = Difficulty.Medium,
                CookingTimeMinutes = 30,
                PrepTimeMinutes = 15,
                Servings = 4,
                Ingredients = @"[
                    ""2 Zwiebeln"",
                    ""3 Knoblauchzehen"",
                    ""1 Paprika"",
                    ""1 Zucchini"",
                    ""2 Karotten"",
                    ""2 EL Currypaste"",
                    ""400ml Kokosmilch"",
                    ""Öl zum Anbraten"",
                    ""Reis zum Servieren""
                ]",
                Instructions = @"1. Zwiebeln und Knoblauch in Öl anbraten.
2. Currypaste hinzugeben und kurz mitbraten.
3. Gemüse (Paprika, Zucchini, Karotten) hinzufügen.
4. Mit Kokosmilch ablöschen.
5. 20 Minuten köcheln lassen.
6. Mit Reis servieren.",
                Calories = 380,
                Protein = 12,
                Carbohydrates = 48,
                Fat = 16,
                AverageRating = 4.3,
                RatingCount = 15
            },
            new Recipe
            {
                Name = "Schokoladen-Brownies",
                Description = "Saftige Schokoladen-Brownies mit knuspriger Kruste.",
                Category = RecipeCategory.Dessert,
                Difficulty = Difficulty.Easy,
                ImagePath = "🍫",
                CookingTimeMinutes = 25,
                PrepTimeMinutes = 15,
                Servings = 12,
                Ingredients = @"[
                    ""200g Zartbitterschokolade"",
                    ""200g Butter"",
                    ""300g Zucker"",
                    ""4 Eier"",
                    ""150g Mehl"",
                    ""50g Kakaopulver"",
                    ""1 Prise Salz"",
                    ""Optional: Walnüsse""
                ]",
                Instructions = @"1. Ofen auf 180°C vorheizen.
2. Schokolade und Butter schmelzen.
3. Zucker und Eier unterrühren.
4. Mehl, Kakao und Salz hinzufügen.
5. Teig in Form geben.
6. 25 Minuten backen (nicht zu lange!).",
                Calories = 320,
                Protein = 4,
                Carbohydrates = 38,
                Fat = 18,
                AverageRating = 4.9,
                RatingCount = 42,
                IsFavorite = true
            }
        };

        // Add ingredients to recipes
        sampleRecipes[0].IngredientList = new List<Ingredient>
        {
            new Ingredient { Name = "Spaghetti", Amount = 500, Unit = "g" },
            new Ingredient { Name = "Speck", Amount = 200, Unit = "g" },
            new Ingredient { Name = "Eier", Amount = 4, Unit = "Stück" },
            new Ingredient { Name = "Parmesan", Amount = 100, Unit = "g" }
        };

        sampleRecipes[1].IngredientList = new List<Ingredient>
        {
            new Ingredient { Name = "Römersalat", Amount = 2, Unit = "Stück" },
            new Ingredient { Name = "Mayonnaise", Amount = 150, Unit = "ml" },
            new Ingredient { Name = "Parmesan", Amount = 50, Unit = "g" },
            new Ingredient { Name = "Weißbrot", Amount = 4, Unit = "Scheiben" }
        };

        sampleRecipes[2].IngredientList = new List<Ingredient>
        {
            new Ingredient { Name = "Mehl", Amount = 250, Unit = "g" },
            new Ingredient { Name = "Milch", Amount = 300, Unit = "ml" },
            new Ingredient { Name = "Eier", Amount = 2, Unit = "Stück" },
            new Ingredient { Name = "Zucker", Amount = 3, Unit = "EL" }
        };

        sampleRecipes[3].IngredientList = new List<Ingredient>
        {
            new Ingredient { Name = "Paprika", Amount = 2, Unit = "Stück" },
            new Ingredient { Name = "Zucchini", Amount = 1, Unit = "Stück" },
            new Ingredient { Name = "Kokosmilch", Amount = 400, Unit = "ml" },
            new Ingredient { Name = "Currypaste", Amount = 2, Unit = "EL" }
        };

        sampleRecipes[4].IngredientList = new List<Ingredient>
        {
            new Ingredient { Name = "Zartbitterschokolade", Amount = 200, Unit = "g" },
            new Ingredient { Name = "Butter", Amount = 150, Unit = "g" },
            new Ingredient { Name = "Zucker", Amount = 200, Unit = "g" },
            new Ingredient { Name = "Eier", Amount = 3, Unit = "Stück" }
        };

        foreach (var recipe in sampleRecipes)
        {
            await SaveRecipeAsync(recipe);
        }
    }
}
