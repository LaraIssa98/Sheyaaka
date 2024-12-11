using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Sheyaaka.Models;

namespace Sheyaaka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BrandController : ControllerBase
    {
        private readonly BrandService _brandService;

        public BrandController(BrandService brandService)
        {
            _brandService = brandService;
        }

        // POST: api/Brand
        [HttpPost]
        public ActionResult<ApiResponse<Brand>> CreateBrand([FromBody] Brand brand)
        {
            try
            {
                var createdBrand = _brandService.CreateBrand(brand);
                return Ok(new ApiResponse<Brand>(createdBrand));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<Brand>(ex.Message));
            }
        }

        // GET: getall
        [HttpGet("GetAllBrands")]
        public ActionResult<ApiResponse<List<Brand>>> GetAllBrands()
        {
            try
            {
                // Call the service to get all brands
                var brands = _brandService.GetAll();

                // Return a successful response with the list of brands
                return Ok(new ApiResponse<List<Brand>>(brands));
            }
            catch (Exception ex)
            {
                // Handle any errors and return a NotFound response
                return NotFound(new ApiResponse<List<Brand>>(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<Brand>> GetBrand(int id)
        {
            try
            {
                var brand = _brandService.GetBrand(id);
                return Ok(new ApiResponse<Brand>(brand));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<Brand>(ex.Message));
            }
        }

        // PUT: api/Brand/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<string>> UpdateBrand(int id, [FromBody] Brand brand)
        {
            try
            {
                _brandService.UpdateBrand(id, brand);
                return Ok(new ApiResponse<string>("Brand updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        // DELETE: api/Brand/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<string>> DeleteBrand(int id)
        {
            try
            {
                _brandService.DeleteBrand(id);
                return Ok(new ApiResponse<string>("Brand deleted successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(ex.Message));
            }
        }

        // POST: api/Brand/unassign-store/{storeId}/{brandId}
        [HttpPost("unassign-store/{storeId}/{brandId}")]
        public ActionResult<ApiResponse<string>> UnassignBrandFromStore(int storeId, int brandId)
        {
            try
            {
                _brandService.UnassignBrandFromStore(storeId, brandId);
                return Ok(new ApiResponse<string>("Brand un-assigned from store successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }
    }
}
