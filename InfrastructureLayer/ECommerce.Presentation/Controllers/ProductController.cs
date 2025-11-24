using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.Common;
using ECommerce.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;   
        public ProductController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // Define action methods to handle product-related requests
        public async Task<ActionResult<PaginationResult<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParam productQueryParam)
        {
           var products = await _serviceManager.ProductServices.GetAllProductAsync(productQueryParam);
           return Ok(products);
        }

        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductServices.GetAllBrandsAsync();
            return Ok(brands);
        }


        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductServices.GetAllTypesAsync();
            return Ok(types);
        }

        [HttpGet("products/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
           var product = await _serviceManager.ProductServices.GetProductByIdAsync(id);
           return Ok(product);
        }
    }
}
