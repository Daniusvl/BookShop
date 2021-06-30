using BookShop.Core.Abstract.Identity;
using BookShop.Core.Models.Authentication;
using BookShop.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService service;

        public AuthController(IAuthenticationService service)
        {
            this.service = service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            RegisterModel model = await service.Register(request, Request);
            return Ok(model);
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AuthenticationModel model = await service.Authenticate(request);
            return Ok(model);
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation(string user_id, string email_token)
        {
            email_token = email_token.Replace(' ', '+');

            await service.ConfirmEmail(user_id, email_token);

            return Ok();
        }

        [Authorize(Policy = RoleConstants.DefaultUserName)]
        [HttpPost("ValidateToken")]
        public IActionResult ValidateToken()
        {
            return Ok();
        }
    }
}
