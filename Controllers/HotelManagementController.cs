using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelManagement.Dtos;
using HotelManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HotelManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelManagementController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<HotelManagementController> logger;
        private readonly IHotelService hotelService;

        public HotelManagementController(
            IConfiguration configuration,
            ILogger<HotelManagementController> logger,
            IHotelService hotelService)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.hotelService = hotelService;
        }

        
        // LOGIN
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await hotelService.LoginAsync(loginDto);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        message = "Invalid Hotel ID or Hotel Name"
                    });
                }

                // Create claims
                var claims = new[]
                {
                    new Claim(
                        JwtRegisteredClaimNames.Sub,
                        user.HotelName),

                    new Claim(
                        JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                };

                // Get JWT key
                var keyValue = configuration["JwtSettings:Key"];

                if (string.IsNullOrEmpty(keyValue))
                {
                    logger.LogError("JWT Key is missing");

                    return StatusCode(500, new
                    {
                        message = "JWT Key is missing"
                    });
                }

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(keyValue));

                var credentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

                // Create token
                var token = new JwtSecurityToken(
                    issuer: configuration["JwtSettings:Issuer"],
                    audience: configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: credentials
                );

                // Convert token to string
                var tokenKey =
                    new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new UserTokenResponse
                {
                    Token = tokenKey
                });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred during hotel login");

                return StatusCode(500, new
                {
                    message = "An error occurred during login"
                });
            }
        }

        
        // GET ALL HOTELS
        
        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await hotelService.GetHotelsAsync();

                return Ok(hotels);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while getting hotels");

                return StatusCode(500, new
                {
                    message = "Error occurred while getting hotels"
                });
            }
        }

        
        // GET HOTEL BY ID
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            try
            {
                var hotel =
                    await hotelService.GetHotelByIdAsync(id);

                if (hotel == null)
                {
                    return NotFound(new
                    {
                        message = "Hotel not found"
                    });
                }

                return Ok(hotel);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while getting hotel with ID {Id}",
                    id);

                return StatusCode(500, new
                {
                    message = "Error occurred while getting hotel"
                });
            }
        }

        
        // CREATE HOTEL
       
        [HttpPost]
        public async Task<IActionResult> CreateHotel(
            [FromBody] HotelDto hotelDto)
        {
            try
            {
                var id =
                    await hotelService.CreateHotelAsync(hotelDto);

                return Ok(new
                {
                    message = "Hotel created successfully",
                    hotelId = id
                });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while creating hotel");

                return StatusCode(500, new
                {
                    message = "Error occurred while creating hotel"
                });
            }
        }

        
        // UPDATE HOTEL
       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(
            int id,
            [FromBody] HotelDto hotelDto)
        {
            try
            {
                var result =
                    await hotelService.UpdateHotelAsync(
                        id,
                        hotelDto);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "Hotel not found"
                    });
                }

                return Ok(new
                {
                    message = "Hotel updated successfully"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while updating hotel with ID {Id}",
                    id);

                return StatusCode(500, new
                {
                    message = "Error occurred while updating hotel"
                });
            }
        }

        // DELETE HOTEL management system
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                var result =
                    await hotelService.DeleteHotelAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "Hotel not found"
                    });
                }

                return Ok(new
                {
                    message = "Hotel deleted successfully"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error occurred while deleting hotel with ID {Id}",
                    id);

                return StatusCode(500, new
                {
                    message = "Error occurred while deleting hotel"
                });
            }
        }
    }
}