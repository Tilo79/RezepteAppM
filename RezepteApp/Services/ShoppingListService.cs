using RezepteApp.Data;
using RezepteApp.Models;

namespace RezepteApp.Services;

public class ShoppingListService
{
    private readonly RecipeDatabase _database;

    public ShoppingListService(RecipeDatabase database)
    {
        _database = database;
    }

    public async Task<List<ShoppingListItem>> GetShoppingListAsync()
    {
        return await _database.GetShoppingListAsync();
    }

    public async Task<int> AddItemAsync(ShoppingListItem item)
    {
        return await _database.SaveShoppingListItemAsync(item);
    }

    public async Task<int> UpdateItemAsync(ShoppingListItem item)
    {
        return await _database.SaveShoppingListItemAsync(item);
    }

    public async Task<int> DeleteItemAsync(ShoppingListItem item)
    {
        return await _database.DeleteShoppingListItemAsync(item);
    }

    public async Task<int> ToggleItemCheckAsync(ShoppingListItem item)
    {
        item.IsChecked = !item.IsChecked;
        return await _database.SaveShoppingListItemAsync(item);
    }

    public async Task<int> ClearCheckedItemsAsync()
    {
        return await _database.ClearCheckedItemsAsync();
    }

    public async Task AddRecipeIngredientsAsync(Recipe recipe, int servings = 0)
    {
        var adjustedServings = servings > 0 ? servings : recipe.Servings;
        var factor = (double)adjustedServings / recipe.Servings;

        foreach (var ingredient in recipe.IngredientList)
        {
            var item = new ShoppingListItem
            {
                Name = ingredient.Name,
                Amount = ingredient.Amount * factor,
                Unit = ingredient.Unit,
                RecipeId = recipe.Id
            };

            await AddItemAsync(item);
        }
    }
}
