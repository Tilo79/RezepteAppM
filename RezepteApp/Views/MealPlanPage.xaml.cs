using RezepteApp.ViewModels;

namespace RezepteApp.Views;

public partial class MealPlanPage : ContentPage
{
    private readonly MealPlanViewModel _viewModel;

    public MealPlanPage(MealPlanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMealPlansCommand.ExecuteAsync(null);
    }
}
