using RezepteApp.Data;
using RezepteApp.Models;

namespace RezepteApp.Services;

public class MealPlanService
{
    private readonly RecipeDatabase _database;

    public MealPlanService(RecipeDatabase database)
    {
        _database = database;
    }

    public async Task<List<MealPlan>> GetWeekPlanAsync(DateTime startDate)
    {
        var endDate = startDate.AddDays(6);
        var mealPlans = await _database.GetMealPlansAsync(startDate, endDate);

        // Load recipe details for each meal plan
        foreach (var mealPlan in mealPlans)
        {
            mealPlan.Recipe = await _database.GetRecipeAsync(mealPlan.RecipeId);
        }

        return mealPlans;
    }

    public async Task<List<MealPlan>> GetMealPlansForDateAsync(DateTime date)
    {
        var startDate = date.Date;
        var endDate = date.Date.AddDays(1);
        var mealPlans = await _database.GetMealPlansAsync(startDate, endDate);

        foreach (var mealPlan in mealPlans)
        {
            mealPlan.Recipe = await _database.GetRecipeAsync(mealPlan.RecipeId);
        }

        return mealPlans;
    }

    public async Task<int> AddMealPlanAsync(MealPlan mealPlan)
    {
        return await _database.SaveMealPlanAsync(mealPlan);
    }

    public async Task<int> UpdateMealPlanAsync(MealPlan mealPlan)
    {
        return await _database.SaveMealPlanAsync(mealPlan);
    }

    public async Task<int> DeleteMealPlanAsync(MealPlan mealPlan)
    {
        return await _database.DeleteMealPlanAsync(mealPlan);
    }
}
