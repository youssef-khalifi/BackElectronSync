using AppSqlite.Data;
using AppSqlite.Features.Product.Queries.GetAlProducts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppSqlite.Features.Product.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Entities.Product>
{
    
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(ApplicationDbContext context, ILogger<GetProductByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Entities.Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
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

            _logger.LogInformation($"Product with ID {product.Id}  found successfully.");

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error find product");
            throw;
        }
    }
}