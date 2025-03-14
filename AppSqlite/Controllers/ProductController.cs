using AppSqlite.Data;
using AppSqlite.Dtos;
using AppSqlite.Features.Product.Commands.CreateProduct;
using AppSqlite.Features.Product.Commands.DeleteProduct;
using AppSqlite.Features.Product.Commands.UpdateProduct;
using AppSqlite.Features.Product.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsManage.Dtos;
using ProductsManage.Features.Product.Queries.GetAlProducts;

namespace AppSqlite.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public ProductController(ApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        /*var products = await _mediator.Send(new GetAlProductQuery());
        return Ok(products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
        }));*/
        
        return Ok( _context.Products.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var query = GetProductByIdQuery.FromDto(id);
        var productCreated = await _mediator.Send(query);

        return Ok(new ProductDto
        {
            Id = productCreated.Id,
            Name = productCreated.Name,
            Description = productCreated.Description,
            Price = productCreated.Price,
        });
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct([FromBody] ProductRequestDto requestDto)
    {
        var command = CreateProductCommand.FromDto(requestDto);
        var createdProduct = await _mediator.Send(command);
        

        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, new ProductDto
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price,
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, [FromBody] UpdateProductDto requestDto)
    {
        var command = UpdateProductCommand.FromDto(id , requestDto);
        var updatedProduct = await _mediator.Send(command);
        return Ok(updatedProduct);
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));

        if (result)
        {
            return NoContent();
        }
        return BadRequest("Product not found");
    }
}
