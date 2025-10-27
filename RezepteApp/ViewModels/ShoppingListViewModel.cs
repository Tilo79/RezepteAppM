using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RezepteApp.Models;
using RezepteApp.Services;
using System.Collections.ObjectModel;

namespace RezepteApp.ViewModels;

public partial class ShoppingListViewModel : ObservableObject
{
    private readonly ShoppingListService _shoppingListService;

    [ObservableProperty]
    private ObservableCollection<ShoppingListItem> shoppingList = new();

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string newItemName = string.Empty;

    public ShoppingListViewModel(ShoppingListService shoppingListService)
    {
        _shoppingListService = shoppingListService;
    }

    [RelayCommand]
    private async Task LoadShoppingListAsync()
    {
        try
        {
            var items = await _shoppingListService.GetShoppingListAsync();
            ShoppingList.Clear();
            foreach (var item in items)
            {
                ShoppingList.Add(item);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", $"Einkaufsliste konnte nicht geladen werden: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadShoppingListAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        if (string.IsNullOrWhiteSpace(NewItemName))
            return;

        var item = new ShoppingListItem
        {
            Name = NewItemName,
            IsChecked = false
        };

        await _shoppingListService.AddItemAsync(item);
        ShoppingList.Add(item);
        NewItemName = string.Empty;
    }

    [RelayCommand]
    private async Task ToggleItemAsync(ShoppingListItem item)
    {
        await _shoppingListService.ToggleItemCheckAsync(item);
    }

    [RelayCommand]
    private async Task DeleteItemAsync(ShoppingListItem item)
    {
        await _shoppingListService.DeleteItemAsync(item);
        ShoppingList.Remove(item);
    }

    [RelayCommand]
    private async Task ClearCheckedItemsAsync()
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Bestätigen",
            "Möchten Sie alle abgehakten Artikel löschen?",
            "Ja", "Nein");

        if (confirm)
        {
            await _shoppingListService.ClearCheckedItemsAsync();
            await LoadShoppingListAsync();
        }
    }
}
