using Microsoft.AspNetCore.Mvc;
using Store.API.Services.Abstractions;
using Store.API.Shared.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync();
            if (products is null) return BadRequest();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            if(product is null) return NotFound();
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (brands is null) return BadRequest();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            if (types is null) return BadRequest();
            return Ok(types);
        }
    }
}
