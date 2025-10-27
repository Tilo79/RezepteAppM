using RezepteApp.ViewModels;

namespace RezepteApp.Views;

public partial class RecipesPage : ContentPage
{
    public RecipesPage(RecipesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is RecipesViewModel viewModel)
        {
            await viewModel.LoadRecipesCommand.ExecuteAsync(null);
        }
    }
}
