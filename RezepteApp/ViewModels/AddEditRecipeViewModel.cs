using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezepteApp.Models;
using RezepteApp.Services;
using System.Collections.ObjectModel;

namespace RezepteApp.ViewModels;

[QueryProperty(nameof(Recipe), "Recipe")]
public partial class AddEditRecipeViewModel : ObservableObject
{
    private readonly RecipeService _recipeService;

    [ObservableProperty]
    private Recipe? recipe;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string selectedCategory = RecipeCategory.Lunch;

    [ObservableProperty]
    private string selectedDifficulty = Difficulty.Medium;

    [ObservableProperty]
    private int cookingTime;

    [ObservableProperty]
    private int prepTime;

    [ObservableProperty]
    private int servings = 4;

    [ObservableProperty]
    private string instructions = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Ingredient> ingredients = new();

    [ObservableProperty]
    private int calories;

    [ObservableProperty]
    private double protein;

    [ObservableProperty]
    private double carbohydrates;

    [ObservableProperty]
    private double fat;

    [ObservableProperty]
    private bool isEditMode;

    public List<string> Categories => RecipeCategory.AllCategories;
    public List<string> Difficulties => Difficulty.AllLevels;

    public AddEditRecipeViewModel(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    partial void OnRecipeChanged(Recipe? value)
    {
        if (value != null)
        {
            IsEditMode = true;
            Name = value.Name;
            Description = value.Description;
            SelectedCategory = value.Category;
            SelectedDifficulty = value.Difficulty;
            CookingTime = value.CookingTimeMinutes;
            PrepTime = value.PrepTimeMinutes;
            Servings = value.Servings;
            Instructions = value.Instructions;
            Calories = value.Calories;
            Protein = value.Protein;
            Carbohydrates = value.Carbohydrates;
            Fat = value.Fat;

            Ingredients.Clear();
            foreach (var ingredient in value.IngredientList)
            {
                Ingredients.Add(ingredient);
            }
        }
        else
        {
            IsEditMode = false;
        }
    }

    [RelayCommand]
    private void AddIngredient()
    {
        Ingredients.Add(new Ingredient());
    }

    [RelayCommand]
    private void RemoveIngredient(Ingredient ingredient)
    {
        Ingredients.Remove(ingredient);
    }

    [RelayCommand]
    private async Task SaveRecipeAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Fehler", "Bitte geben Sie einen Namen ein.", "OK");
            return;
        }

        if (Ingredients.Count == 0)
        {
            await Shell.Current.DisplayAlert("Fehler", "Bitte fügen Sie mindestens eine Zutat hinzu.", "OK");
            return;
        }

        try
        {
            var recipeToSave = Recipe ?? new Recipe();
            recipeToSave.Name = Name;
            recipeToSave.Description = Description;
            recipeToSave.Category = SelectedCategory;
            recipeToSave.Difficulty = SelectedDifficulty;
            recipeToSave.CookingTimeMinutes = CookingTime;
            recipeToSave.PrepTimeMinutes = PrepTime;
            recipeToSave.Servings = Servings;
            recipeToSave.Instructions = Instructions;
            recipeToSave.Calories = Calories;
            recipeToSave.Protein = Protein;
            recipeToSave.Carbohydrates = Carbohydrates;
            recipeToSave.Fat = Fat;
            recipeToSave.IngredientList = Ingredients.ToList();

            await _recipeService.SaveRecipeAsync(recipeToSave);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", $"Rezept konnte nicht gespeichert werden: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
