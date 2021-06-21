using BookShop.Core.Abstract.Identity;
using BookShop.Core.Models.Authentication;
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

        [HttpPost("/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            RegisterModel model = await service.Register(request);
            if (!model.Success)
            {
                return BadRequest(model.PasswordErrors);
            }
            return Ok(model);
        }

        [HttpPost("/Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            AuthenticationModel model = await service.Authenticate(request);
            if (!model.Success)
            {
                return BadRequest();
            }
            return Ok(model);
        }
    }
}
