using AppSqlite.Dtos;
using MediatR;
using ProductsManage.Dtos;

namespace AppSqlite.Features.Product.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Entities.Product>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } 
    
    public static UpdateProductCommand FromDto(int id , UpdateProductDto dto)
    {
        return new UpdateProductCommand
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            
        };
    }
}