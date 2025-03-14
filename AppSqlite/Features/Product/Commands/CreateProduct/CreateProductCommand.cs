using MediatR;
using ProductsManage.Dtos;

namespace AppSqlite.Features.Product.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Entities.Product>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    

    public static CreateProductCommand FromDto(ProductRequestDto dto)
    {
        return new CreateProductCommand
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            
        };
    }
}