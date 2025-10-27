using RezepteApp.Views;

namespace RezepteApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(RecipeDetailPage), typeof(RecipeDetailPage));
        Routing.RegisterRoute(nameof(AddEditRecipePage), typeof(AddEditRecipePage));
    }
}
