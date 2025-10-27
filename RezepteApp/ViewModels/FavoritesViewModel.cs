using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezepteApp.Models;
using RezepteApp.Services;
using System.Collections.ObjectModel;

namespace RezepteApp.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;

    [ObservableProperty]
    private ObservableCollection<Recipe> favoriteRecipes = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private bool hasFavorites;

    public FavoritesViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public async Task LoadFavoritesPublicAsync()
    {
        await LoadFavoritesAsync();
    }

    [RelayCommand]
    private async Task LoadFavoritesAsync()
    {
        try
        {
            var favorites = await _recipeService.GetFavoriteRecipesAsync();
            FavoriteRecipes.Clear();
            foreach (var recipe in favorites)
            {
                FavoriteRecipes.Add(recipe);
            }
            HasFavorites = FavoriteRecipes.Count > 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", $"Favoriten konnten nicht geladen werden: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadFavoritesAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task NavigateToRecipeDetailAsync(Recipe recipe)
    {
        if (recipe == null) return;

        await Shell.Current.GoToAsync($"RecipeDetailPage", new Dictionary<string, object>
        {
            { "Recipe", recipe }
        });
    }

    [RelayCommand]
    private async Task RemoveFromFavoritesAsync(Recipe recipe)
    {
        await _recipeService.ToggleFavoriteAsync(recipe);
        FavoriteRecipes.Remove(recipe);
        HasFavorites = FavoriteRecipes.Count > 0;
    }
}
