using Microsoft.AspNetCore.Mvc;
using OrderTrack.Application.DTOs.Products;
using OrderTrack.Application.Interfaces;

namespace OrderTrack.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponseDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(
        CreateProductDto dto,
        CancellationToken cancellationToken)
    {
        var createdProduct = await _productService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            createdProduct);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponseDto>> Update(
    Guid id,
    UpdateProductDto dto,
    CancellationToken cancellationToken)
    {
        var updatedProduct = await _productService.UpdateAsync(
            id,
            dto,
            cancellationToken);

        if (updatedProduct is null)
        {
            return NotFound();
        }

        return Ok(updatedProduct);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
    Guid id,
    CancellationToken cancellationToken)
    {
        var result = await _productService.DeactivateAsync(id, cancellationToken);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}