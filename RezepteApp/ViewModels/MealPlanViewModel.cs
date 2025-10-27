using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezepteApp.Models;
using RezepteApp.Services;
using System.Collections.ObjectModel;

namespace RezepteApp.ViewModels;

public partial class MealPlanViewModel : ObservableObject
{
    private readonly MealPlanService _mealPlanService;
    private readonly RecipeService _recipeService;

    [ObservableProperty]
    private ObservableCollection<MealPlan> weekMealPlans = new();

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private bool isRefreshing;

    public MealPlanViewModel(MealPlanService mealPlanService, RecipeService recipeService)
    {
        _mealPlanService = mealPlanService;
        _recipeService = recipeService;
    }

    [RelayCommand]
    private async Task LoadMealPlansAsync()
    {
        try
        {
            var startOfWeek = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek + (int)DayOfWeek.Monday);
            var mealPlans = await _mealPlanService.GetWeekPlanAsync(startOfWeek);

            WeekMealPlans.Clear();
            foreach (var plan in mealPlans)
            {
                WeekMealPlans.Add(plan);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", $"Wochenplan konnte nicht geladen werden: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadMealPlansAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task AddMealPlanAsync()
    {
        try
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            if (!recipes.Any())
            {
                await Shell.Current.DisplayAlert("Info", "Bitte fügen Sie erst Rezepte hinzu.", "OK");
                return;
            }

            var recipeNames = recipes.Select(r => r.Name).ToArray();
            var selectedRecipe = await Shell.Current.DisplayActionSheet(
                "Rezept auswählen",
                "Abbrechen",
                null,
                recipeNames);

            if (selectedRecipe == null || selectedRecipe == "Abbrechen")
                return;

            var recipe = recipes.FirstOrDefault(r => r.Name == selectedRecipe);
            if (recipe == null)
                return;

            var mealType = await Shell.Current.DisplayActionSheet(
                "Mahlzeit auswählen",
                "Abbrechen",
                null,
                MealType.AllTypes.ToArray());

            if (mealType == null || mealType == "Abbrechen")
                return;

            var mealPlan = new MealPlan
            {
                RecipeId = recipe.Id,
                Date = SelectedDate,
                MealType = mealType,
                Servings = recipe.Servings,
                Recipe = recipe
            };

            await _mealPlanService.AddMealPlanAsync(mealPlan);
            await LoadMealPlansAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteMealPlanAsync(MealPlan mealPlan)
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Löschen bestätigen",
            "Möchten Sie diesen Eintrag aus dem Wochenplan entfernen?",
            "Ja", "Nein");

        if (confirm)
        {
            await _mealPlanService.DeleteMealPlanAsync(mealPlan);
            WeekMealPlans.Remove(mealPlan);
        }
    }

    [RelayCommand]
    private async Task PreviousWeekAsync()
    {
        SelectedDate = SelectedDate.AddDays(-7);
        await LoadMealPlansAsync();
    }

    [RelayCommand]
    private async Task NextWeekAsync()
    {
        SelectedDate = SelectedDate.AddDays(7);
        await LoadMealPlansAsync();
    }
}
