using AppSqlite.Data;
using AppSqlite.Entities;
using Microsoft.EntityFrameworkCore;
using ProductsManage.Dtos;

namespace AppSqlite.Services.ServicesImpl;

public class SyncService : ISyncService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SyncService> _logger;


    public SyncService(ApplicationDbContext context, ILogger<SyncService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SyncResult> SyncProductsFromSqlite(List<Product> products)
    {
        var result = new SyncResult { Success = true };
        try
        {
            foreach (var sqliteProduct in products)
            {
                // Check if product exists in SQL Server
                var sqlServerProduct = await _context.Products
                    .IgnoreQueryFilters() // Include soft-deleted items
                    .FirstOrDefaultAsync(p => p.SyncId == sqliteProduct.SyncId);

                if (sqlServerProduct == null)
                {
                    // Product doesn't exist in SQL Server, add it
                    if (!sqliteProduct.IsDeleted)
                    {
                        var newProduct = new Product
                        {
                            SyncId = sqliteProduct.SyncId,
                            Name = sqliteProduct.Name,
                            Description = sqliteProduct.Description,
                            Price = sqliteProduct.Price,
                            CreatedAt = sqliteProduct.CreatedAt,
                            UpdatedAt = sqliteProduct.UpdatedAt,
                            IsDeleted = sqliteProduct.IsDeleted,
                            IsSync = true,
                            LastSyncAt = DateTime.UtcNow
                        };
                        
                        
                        await _context.Products.AddAsync(newProduct);
                        result.Added++;
                    }
                }
                else
                {
                    // Product exists in SQL Server, update it
                    if (sqliteProduct.IsDeleted)
                    {
                        // Mark as deleted in SQL Server if deleted in SQLite
                        sqlServerProduct.IsDeleted = true;
                        sqlServerProduct.UpdatedAt = DateTime.UtcNow;
                        sqlServerProduct.IsSync = true;
                        sqlServerProduct.LastSyncAt = DateTime.UtcNow;
                        
                        result.Deleted++;
                    }
                    else
                    {
                        // Update existing record
                        sqlServerProduct.Name = sqliteProduct.Name;
                        sqlServerProduct.Description = sqliteProduct.Description;
                        sqlServerProduct.Price = sqliteProduct.Price;
                        sqlServerProduct.UpdatedAt = DateTime.UtcNow;
                        sqlServerProduct.IsSync = true;
                        sqlServerProduct.LastSyncAt = DateTime.UtcNow;
                        result.Updated++;
                    }

                    
                    _context.Products.Update(sqlServerProduct);
                }
                
            }


            var sqlServerTransaction = await _context.Database.BeginTransactionAsync();


            try
            {
                await _context.SaveChangesAsync();


                await sqlServerTransaction.CommitAsync();

            }
            catch
            {
                await sqlServerTransaction.RollbackAsync();
                throw;
            }


            result.Message =
                $"Sync completed successfully. Added: {result.Added}, Updated: {result.Updated}, Deleted: {result.Deleted}";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Sync failed: {ex.Message}";
            _logger.LogError(ex, "Error during database synchronization");
        }
        
        return result;
    }
}


