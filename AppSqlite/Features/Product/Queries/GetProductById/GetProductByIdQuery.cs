using MediatR;

namespace AppSqlite.Features.Product.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<Entities.Product>
{
    
    public int Id { get; set; }
    
    public static GetProductByIdQuery FromDto(int productId)
    {
        return new GetProductByIdQuery
        {
            Id = productId
            
        };
    }
}