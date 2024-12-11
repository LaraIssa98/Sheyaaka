using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Sheyaaka.Models;
using Sheyaaka.Services;

namespace Sheyaaka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult<ApiResponse<Product>> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new ApiResponse<Product>("Product cannot be null."));
            }

            try
            {
                var createdProduct = _productService.CreateProduct(product);
                return Ok(new ApiResponse<Product>(createdProduct));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<Product>(ex.Message));
            }
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<Product>> GetProduct(int id)
        {
            try
            {
                var product = _productService.GetProduct(id);
                return Ok(new ApiResponse<Product>(product));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<Product>("Product not found."));
            }
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<string>> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest(new ApiResponse<string>("Product ID mismatch."));
            }

            try
            {
                _productService.UpdateProduct(id, product);
                return Ok(new ApiResponse<string>("Product updated successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>("Product not found."));
            }
        }

        // DELETE: api/Product/{id} (Soft Delete)
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<string>> DeleteProduct(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return Ok(new ApiResponse<string>("Product deleted successfully."));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>("Product not found."));
            }
        }

        // GET: api/Product/recover/{id} (Recover Deleted Product)
        [HttpGet("recover/{id}")]
        public ActionResult<ApiResponse<Product>> RecoverProduct(int id)
        {
            try
            {
                var product = _productService.RecoverProduct(id);
                return Ok(new ApiResponse<Product>(product));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<Product>("Product not found or not deleted."));
            }
        }

        // GET: api/Product/all (Get all products, including recovered ones)
        [HttpGet("all")]
        public ActionResult<ApiResponse<IEnumerable<Product>>> GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(new ApiResponse<IEnumerable<Product>>(products));
        }
    }
}
