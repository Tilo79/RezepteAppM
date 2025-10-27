using SQLite;
using RezepteApp.Models;

namespace RezepteApp.Data;

public class RecipeDatabase
{
    private SQLiteAsyncConnection? _database;

    public async Task InitializeAsync()
    {
        if (_database != null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "recipes.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<Recipe>();
        await _database.CreateTableAsync<ShoppingListItem>();
        await _database.CreateTableAsync<MealPlan>();
    }

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database == null)
            await InitializeAsync();
        return _database!;
    }

    #region Recipe Operations

    public async Task<List<Recipe>> GetRecipesAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Recipe>().OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public async Task<Recipe?> GetRecipeAsync(int id)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Recipe>().Where(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Recipe>> GetFavoriteRecipesAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Recipe>().Where(r => r.IsFavorite).ToListAsync();
    }

    public async Task<List<Recipe>> SearchRecipesAsync(string searchText)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Recipe>()
            .Where(r => r.Name.Contains(searchText) || r.Description.Contains(searchText))
            .ToListAsync();
    }

    public async Task<List<Recipe>> GetRecipesByCategoryAsync(string category)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<Recipe>().Where(r => r.Category == category).ToListAsync();
    }

    public async Task<int> SaveRecipeAsync(Recipe recipe)
    {
        var db = await GetDatabaseAsync();
        recipe.UpdatedAt = DateTime.Now;

        if (recipe.Id != 0)
            return await db.UpdateAsync(recipe);
        else
            return await db.InsertAsync(recipe);
    }

    public async Task<int> DeleteRecipeAsync(Recipe recipe)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(recipe);
    }

    #endregion

    #region Shopping List Operations

    public async Task<List<ShoppingListItem>> GetShoppingListAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<ShoppingListItem>().OrderBy(i => i.IsChecked).ToListAsync();
    }

    public async Task<int> SaveShoppingListItemAsync(ShoppingListItem item)
    {
        var db = await GetDatabaseAsync();

        if (item.Id != 0)
            return await db.UpdateAsync(item);
        else
            return await db.InsertAsync(item);
    }

    public async Task<int> DeleteShoppingListItemAsync(ShoppingListItem item)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(item);
    }

    public async Task<int> ClearCheckedItemsAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.ExecuteAsync("DELETE FROM shopping_list WHERE IsChecked = 1");
    }

    #endregion

    #region Meal Plan Operations

    public async Task<List<MealPlan>> GetMealPlansAsync(DateTime startDate, DateTime endDate)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<MealPlan>()
            .Where(mp => mp.Date >= startDate && mp.Date <= endDate)
            .OrderBy(mp => mp.Date)
            .ToListAsync();
    }

    public async Task<int> SaveMealPlanAsync(MealPlan mealPlan)
    {
        var db = await GetDatabaseAsync();

        if (mealPlan.Id != 0)
            return await db.UpdateAsync(mealPlan);
        else
            return await db.InsertAsync(mealPlan);
    }

    public async Task<int> DeleteMealPlanAsync(MealPlan mealPlan)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(mealPlan);
    }

    #endregion
}
