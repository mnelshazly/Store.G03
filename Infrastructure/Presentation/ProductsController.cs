﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet] // GET: /api/products
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await serviceManager.ProductService.GetAllProductsAsync();
            if (result is null) return BadRequest(); // 400

            return Ok(result); // 200
        }

        [HttpGet("{id}")] // GET: /api/products/12
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) return NotFound(); // 404

            return Ok(result); // 200
        }

        [HttpGet("Brands")] // GET: /api/products/Brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return NotFound(); // 404

            return Ok(result); // 200
        }

        [HttpGet("Types")] // GET: /api/products/Types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return NotFound(); // 404

            return Ok(result); // 200
        }
    }
}
