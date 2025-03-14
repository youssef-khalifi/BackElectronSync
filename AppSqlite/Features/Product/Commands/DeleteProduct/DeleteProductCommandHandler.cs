using AppSqlite.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppSqlite.Features.Product.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DeleteProductCommandHandler> _logger;


    public  DeleteProductCommandHandler(ApplicationDbContext context, ILogger<DeleteProductCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning($"product with ID {request.Id} not found for deletion");
                return false;
            }

           product.IsDeleted = true;
           product.IsSync = false;
           product.UpdatedAt = DateTime.Now;
           await _context.SaveChangesAsync(cancellationToken);
           return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting Collection with ID {request.Id}");
            throw;
        }
    }
}