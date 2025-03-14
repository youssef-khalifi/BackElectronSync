using AppSqlite.Data;
using MediatR;

using AppSqlite.Entities;
namespace AppSqlite.Features.Product.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand ,Entities.Product>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(ApplicationDbContext context, ILogger<CreateProductCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Entities.Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = new Entities.Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CreatedAt = DateTime.UtcNow,
                IsSync = false,
                IsDeleted = false,
                SyncId = Guid.NewGuid()
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Created new product with ID {product.Id}");

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new product");
            throw;
        }
    }
    
}