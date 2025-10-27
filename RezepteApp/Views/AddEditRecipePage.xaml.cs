using RezepteApp.ViewModels;

namespace RezepteApp.Views;

public partial class AddEditRecipePage : ContentPage
{
    public AddEditRecipePage(AddEditRecipeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
