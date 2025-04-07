using AppSqlite.Data;
using AppSqlite.Dtos;
using AppSqlite.Entities;
using AppSqlite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppSqlite.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SyncController : ControllerBase
{
    
    private readonly ApplicationDbContext _context;
    private readonly ISyncService _syncService;

    public SyncController(ApplicationDbContext context, ISyncService syncService)
    {
        _context = context;
        _syncService = syncService;
    }
    
    [HttpPost("sync-sqlite")]
    public async Task<IActionResult> GetProductsToSync([FromBody] List<Product> products)
    {
       
      return Ok(await _syncService.SyncProductsFromSqlite(products));
      
      return Ok(new { message = "Success" });
    }

    [HttpGet("sync-sqlserver")]
    public async Task<IActionResult> GetUnSyncProducts()
    {
        var products = await _context.Products
            .FromSqlRaw("SELECT * FROM products WHERE isSync = 0 ")
            .IgnoreQueryFilters()
            .ToListAsync();

        return Ok(products);
    }

    [HttpPost("mark-sync")]
    public async Task<IActionResult> UpdateProducts([FromBody] Product request)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.SyncId == request.SyncId);

        if (product == null)
        {
            return Ok(new { message = "Product not found" });
        }
        product.LastSyncAt = request.LastSyncAt;
        product.IsSync = true;
        await _context.SaveChangesAsync();
        return Ok(product);
        
        
    }

    
    [HttpPost("mark")]
    public async Task<IActionResult> sync([FromBody] Sync request)
    {
        var product = await _context.Products
            .IgnoreQueryFilters().FirstOrDefaultAsync(p => p.SyncId == request.SyncId);

        if (product == null)
        {
            return Ok(new { message = "Product not found" });
        }
        Console.WriteLine( request.LastSyncAt);
        Console.WriteLine( request.SyncId);
        product.LastSyncAt = request.LastSyncAt;
        product.IsSync = true;
        await _context.SaveChangesAsync();
        return Ok(product);
        
        
        
    }

   


}