using AppSqlite.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppSqlite.Features.Product.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand  , Entities.Product>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(ApplicationDbContext context, ILogger<UpdateProductCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Entities.Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Find the product by ID
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            // Check if the product exists
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {request.Id} not found.");
                return null;
            }

            // Update product details, preserving current values if null in the request
            product.Name = request.Name ?? product.Name;
            product.Description = request.Description ?? product.Description;
            product.Price = request?.Price ?? product.Price;
            product.UpdatedAt = DateTime.UtcNow;
            product.IsSync = false;


            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Product with ID {product.Id} updated successfully.");

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product");
            throw;
        }
    }
    
}