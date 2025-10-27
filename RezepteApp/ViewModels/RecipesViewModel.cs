using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezepteApp.Models;
using RezepteApp.Services;
using RezepteApp.Views;
using System.Collections.ObjectModel;

namespace RezepteApp.ViewModels;

public partial class RecipesViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;
    private readonly ShoppingListService _shoppingListService;

    [ObservableProperty]
    private ObservableCollection<Recipe> recipes = new();

    [ObservableProperty]
    private ObservableCollection<Recipe> filteredRecipes = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedCategory = "Alle";

    [ObservableProperty]
    private string selectedDifficulty = "Alle";

    [ObservableProperty]
    private bool isRefreshing;

    public List<string> Categories { get; }
    public List<string> Difficulties { get; }

    public RecipesViewModel(RecipeService recipeService, ShoppingListService shoppingListService)
    {
        _recipeService = recipeService;
        _shoppingListService = shoppingListService;

        Categories = new List<string> { "Alle" }.Concat(RecipeCategory.AllCategories).ToList();
        Difficulties = new List<string> { "Alle" }.Concat(Difficulty.AllLevels).ToList();
    }

    [RelayCommand]
    private async Task LoadRecipesAsync()
    {
        try
        {
            var recipeList = await _recipeService.GetAllRecipesAsync();
            Recipes.Clear();
            foreach (var recipe in recipeList)
            {
                Recipes.Add(recipe);
            }
            ApplyFilters();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", $"Rezepte konnten nicht geladen werden: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadRecipesAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        ApplyFilters();
    }

    [RelayCommand]
    private void FilterByCategory(string category)
    {
        SelectedCategory = category;
        ApplyFilters();
    }

    [RelayCommand]
    private void FilterByDifficulty(string difficulty)
    {
        SelectedDifficulty = difficulty;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = Recipes.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(r =>
                r.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                r.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedCategory != "Alle")
        {
            filtered = filtered.Where(r => r.Category == SelectedCategory);
        }

        if (SelectedDifficulty != "Alle")
        {
            filtered = filtered.Where(r => r.Difficulty == SelectedDifficulty);
        }

        FilteredRecipes.Clear();
        foreach (var recipe in filtered)
        {
            FilteredRecipes.Add(recipe);
        }
    }

    [RelayCommand]
    private async Task NavigateToRecipeDetailAsync(Recipe recipe)
    {
        if (recipe == null) return;

        await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}", new Dictionary<string, object>
        {
            { "Recipe", recipe }
        });
    }

    [RelayCommand]
    private async Task NavigateToAddRecipeAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddEditRecipePage));
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync(Recipe recipe)
    {
        await _recipeService.ToggleFavoriteAsync(recipe);
        ApplyFilters();
    }

    [RelayCommand]
    private async Task DeleteRecipeAsync(Recipe recipe)
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Löschen bestätigen",
            $"Möchten Sie das Rezept '{recipe.Name}' wirklich löschen?",
            "Ja", "Nein");

        if (confirm)
        {
            await _recipeService.DeleteRecipeAsync(recipe);
            Recipes.Remove(recipe);
            ApplyFilters();
        }
    }
}
