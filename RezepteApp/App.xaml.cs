using RezepteApp.Data;
using RezepteApp.Services;

namespace RezepteApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        // Force Light Theme
        UserAppTheme = AppTheme.Light;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override async void OnStart()
    {
        base.OnStart();

        // Initialize database and seed data
        var database = Handler?.MauiContext?.Services.GetService<RecipeDatabase>();
        if (database != null)
        {
            await database.InitializeAsync();

            var recipeService = Handler?.MauiContext?.Services.GetService<RecipeService>();
            if (recipeService != null)
            {
                await recipeService.SeedDataAsync();
            }
        }
    }
}