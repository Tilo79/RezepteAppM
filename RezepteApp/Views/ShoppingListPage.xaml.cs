using RezepteApp.ViewModels;
using RezepteApp.Models;

namespace RezepteApp.Views;

public partial class ShoppingListPage : ContentPage
{
    private readonly ShoppingListViewModel _viewModel;

    public ShoppingListPage(ShoppingListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadShoppingListCommand.ExecuteAsync(null);
    }

    private async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is ShoppingListItem item)
        {
            await _viewModel.ToggleItemCommand.ExecuteAsync(item);
        }
    }
}
