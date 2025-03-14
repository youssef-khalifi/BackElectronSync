using MediatR;

namespace ProductsManage.Features.Product.Queries.GetAlProducts;

public class GetAlProductQuery : IRequest<IEnumerable<AppSqlite.Entities.Product>>
{
    
}