using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Sheyaaka.Models;
using System.Security.Claims;

namespace Sheyaaka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StoreController : ControllerBase
    {
        private readonly StoreService _storeService;

        public StoreController(StoreService storeService)
        {
            _storeService = storeService;
        }

        // POST: api/Store
        [HttpPost]
        public ActionResult<ApiResponse<Store>> CreateStore([FromBody] Store store)
        {
            if (store == null)
            {
                return BadRequest(new ApiResponse<Store>("Store cannot be null."));
            }

            try
            {
                var userid = GetUserIdFromClaims();
                var createdStore = _storeService.CreateStore(store);
                return CreatedAtAction(nameof(GetStore), new { id = createdStore.StoreID }, new ApiResponse<Store>(createdStore));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<Store>(ex.Message));
            }
        }

        // GET: api/Store/{id}
        [HttpGet("GetStoreById/{id}")]
        public ActionResult<ApiResponse<Store>> GetStore(int id)
        {
            try
            {
                var store = _storeService.GetStore(id);
                if (store == null)
                {
                    return NotFound(new ApiResponse<Store>("Store not found."));
                }
                return Ok(new ApiResponse<Store>(store));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<Store>(ex.Message));
            }
        }

        // PUT: api/Store/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<string>> UpdateStore(int id, [FromBody] Store store)
        {
            if (id != store.StoreID)
            {
                return BadRequest(new ApiResponse<string>("Store ID mismatch."));
            }

            try
            {
                _storeService.UpdateStore(id, store);
                return Ok(new ApiResponse<string>("Store updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        // DELETE: api/Store/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<string>> DeleteStore(int id)
        {
            try
            {
                _storeService.DeleteStore(id);
                return Ok(new ApiResponse<string>("Store deleted successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(ex.Message));
            }
        }

        // POST: api/Store/{storeId}/Address
        [HttpPost("{storeId}/AddAddress")]
        public async Task<ActionResult<ApiResponse<Address>>> AddAddress([FromBody] Address address)
        {
            if (address == null)
            {
                return BadRequest(new ApiResponse<Address>("Address cannot be null."));
            }

            try
            {            
                

                // Proceed with adding the address if user is authorized
                var createdAddress =  _storeService.AddAddress( address);
                return CreatedAtAction(nameof(GetAddresses), new { storeId = address.StoreID }, new ApiResponse<Address>(createdAddress));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<Address>(ex.Message));
            }
        }
        // Get address by ID
        [HttpGet("{storeId}")]
        public async Task<IActionResult> GetsingleAddress(int addressID)
        {
            var userId = GetUserIdFromClaims();
            var addresses = await _storeService.GetSingleAddress(addressID, userId);

            return Ok(addresses);
        }
        [HttpPut("{addressID}")]//done
        public async Task<IActionResult> UpdateAddress( [FromBody] Address updatedAddress)
        {
            try
            {
                var userId = GetUserIdFromClaims();
                await _storeService.UpdateAddress(updatedAddress, userId);
                return Ok("Address updated successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{storeId}/GetAllAddresses")]
        public ActionResult<ApiResponse<IEnumerable<Address>>> GetAddresses(int storeId)
        {
            try
            {
                int associatedUserid = GetUserIdFromClaims();

                // Ensure associatedUser is valid and obtained properly
                if (associatedUserid == 0)
                    return Unauthorized(new ApiResponse<IEnumerable<Address>>("User not authenticated."));
                var addresses = _storeService.GetAllAddresses(storeId, associatedUserid);
                return Ok(new ApiResponse<IEnumerable<Address>>(addresses));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<IEnumerable<Address>>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<Address>>("An error occurred: " + ex.Message));
            }
        }
        [HttpDelete("DeleteAddressById/{addressID}")]

        public async Task<IActionResult> DeleteAddress(int addressID)
        {
            try
            {
                // Extract the user ID from claims
                var userId = GetUserIdFromClaims();

                // Call the service to delete the address
                await _storeService.DeleteAddress(addressID, userId);

                return Ok("Address deleted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        private int GetUserIdFromClaims()
        {
            // Assuming the UserID is stored in the JWT claims
            return int.Parse(User.FindFirst("UserID")?.Value ?? "0");
        }
    }
}
