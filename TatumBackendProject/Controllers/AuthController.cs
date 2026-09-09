using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TatumBackendProject.DTOs;
using TatumBackendProject.Services;

namespace TatumBackendProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        //==================================
        // CUSTOMER REGISTRATION
        // =================================

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request, CancellationToken ct)
        {
            try
            {
                var response = await _auth.RegisterAsync(request, ct);

                if (!response.Success)
                {
                    return Conflict(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("An error occured");
            }
        }
    }
}
