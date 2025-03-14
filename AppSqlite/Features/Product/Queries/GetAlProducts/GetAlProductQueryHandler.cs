using AppSqlite.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsManage.Features.Product.Queries.GetAlProducts;

namespace AppSqlite.Features.Product.Queries.GetAlProducts;

public class GetAlProductQueryHandler : IRequestHandler<GetAlProductQuery , IEnumerable<Entities.Product>>
{
    
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetAlProductQueryHandler> _logger;

    public GetAlProductQueryHandler(ApplicationDbContext context, ILogger<GetAlProductQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Entities.Product>> Handle(GetAlProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Products
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            throw;
        }
    }
}