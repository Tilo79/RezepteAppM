using SQLite;

namespace RezepteApp.Models;

[Table("shopping_list")]
public class ShoppingListItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(200), NotNull]
    public string Name { get; set; } = string.Empty;

    public double Amount { get; set; }

    public string Unit { get; set; } = string.Empty;

    public bool IsChecked { get; set; }

    public int? RecipeId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Ignore]
    public string DisplayText => Amount > 0 
        ? $"{Amount} {Unit} {Name}" 
        : Name;
}
