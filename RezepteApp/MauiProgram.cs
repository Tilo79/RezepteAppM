using Microsoft.Extensions.Logging;
using RezepteApp.Data;
using RezepteApp.Services;
using RezepteApp.ViewModels;
using RezepteApp.Views;
using CommunityToolkit.Maui;

namespace RezepteApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Force Light Theme
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping("ForceLight", (handler, view) =>
        {
            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = AppTheme.Light;
            }
        });

        // Register Database
        builder.Services.AddSingleton<RecipeDatabase>();

        // Register Services
        builder.Services.AddSingleton<RecipeService>();
        builder.Services.AddSingleton<ShoppingListService>();
        builder.Services.AddSingleton<MealPlanService>();

        // Register ViewModels
        builder.Services.AddTransient<RecipesViewModel>();
        builder.Services.AddTransient<RecipeDetailViewModel>();
        builder.Services.AddTransient<AddEditRecipeViewModel>();
        builder.Services.AddTransient<ShoppingListViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<MealPlanViewModel>();

        // Register Views
        builder.Services.AddTransient<RecipesPage>();
        builder.Services.AddTransient<RecipeDetailPage>();
        builder.Services.AddTransient<AddEditRecipePage>();
        builder.Services.AddTransient<ShoppingListPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<MealPlanPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
