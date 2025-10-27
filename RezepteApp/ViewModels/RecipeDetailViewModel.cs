using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezepteApp.Models;
using RezepteApp.Services;
using RezepteApp.Views;

namespace RezepteApp.ViewModels;

[QueryProperty(nameof(Recipe), "Recipe")]
public partial class RecipeDetailViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;
    private readonly ShoppingListService _shoppingListService;
    private readonly MealPlanService _mealPlanService;

    [ObservableProperty]
    private Recipe? recipe;

    [ObservableProperty]
    private bool isFavorite;

    public RecipeDetailViewModel(
        RecipeService recipeService,
        ShoppingListService shoppingListService,
        MealPlanService mealPlanService)
    {
        _recipeService = recipeService;
        _shoppingListService = shoppingListService;
        _mealPlanService = mealPlanService;
    }

    partial void OnRecipeChanged(Recipe? value)
    {
        if (value != null)
        {
            IsFavorite = value.IsFavorite;
        }
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync()
    {
        if (Recipe == null) return;

        await _recipeService.ToggleFavoriteAsync(Recipe);
        IsFavorite = Recipe.IsFavorite;
    }

    [RelayCommand]
    private async Task AddToShoppingListAsync()
    {
        if (Recipe == null) return;

        try
        {
            await _shoppingListService.AddRecipeIngredientsAsync(Recipe);
            await Shell.Current.DisplayAlert(
                "Erfolgreich",
                "Zutaten wurden zur Einkaufsliste hinzugefügt!",
                "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task RateRecipeAsync()
    {
        if (Recipe == null) return;

        var rating = await Shell.Current.DisplayActionSheet(
            "Bewertung abgeben",
            "Abbrechen",
            null,
            "⭐ 1 Stern",
            "⭐⭐ 2 Sterne",
            "⭐⭐⭐ 3 Sterne",
            "⭐⭐⭐⭐ 4 Sterne",
            "⭐⭐⭐⭐⭐ 5 Sterne");

        if (rating != null && rating != "Abbrechen")
        {
            var ratingValue = rating.Count(c => c == '⭐');
            await _recipeService.RateRecipeAsync(Recipe, ratingValue);

            // Reload recipe to get updated rating
            Recipe = await _recipeService.GetRecipeByIdAsync(Recipe.Id);
        }
    }

    [RelayCommand]
    private async Task EditRecipeAsync()
    {
        if (Recipe == null) return;

        await Shell.Current.GoToAsync(nameof(AddEditRecipePage), new Dictionary<string, object>
        {
            { "Recipe", Recipe }
        });
    }

    [RelayCommand]
    private async Task DeleteRecipeAsync()
    {
        if (Recipe == null) return;

        var confirm = await Shell.Current.DisplayAlert(
            "Löschen bestätigen",
            $"Möchten Sie das Rezept '{Recipe.Name}' wirklich löschen?",
            "Ja", "Nein");

        if (confirm)
        {
            await _recipeService.DeleteRecipeAsync(Recipe);
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    private async Task ShareRecipeAsync()
    {
        if (Recipe == null) return;

        var text = $"Schau dir dieses Rezept an: {Recipe.Name}\n\n{Recipe.Description}";
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Rezept teilen"
        });
    }
}
