namespace AppSqlite.Entities;

public class Product
{
    public int Id { get; set; } // Nullable to handle auto-generated ID in SQLite
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public bool IsSync { get; set; }
    public bool IsDeleted { get; set; } // To track if the product is marked as deleted
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastSyncAt { get; set; }
    public Guid SyncId { get; set; }
}