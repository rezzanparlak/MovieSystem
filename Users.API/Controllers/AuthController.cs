using CORE.APP.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.APP.Features.Auth;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">Register request containing user information.</param>
        /// <returns>CommandResponse indicating success or failure.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                        return Ok(response);

                    return BadRequest(response);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register exception");
                return StatusCode(500, "Register failed.");
            }
        }
    }
}