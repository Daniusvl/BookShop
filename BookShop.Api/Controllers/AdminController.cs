using BookShop.Core;
using BookShop.Core.Abstract.Identity;
using BookShop.Core.Models.User;
using BookShop.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = RoleConstants.AdministratorName)]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService userService;

        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("ChangeRole")]
        public async Task<ActionResult> ChangeRole([FromBody] ChangeRoleRequest request)
        {
            switch (request.Role)
            {
                case "DefaultUser":
                     await userService.ChangeRole(request.UserId, Role.DefaultUser);
                    return Ok();
                case "Manager":
                    await userService.ChangeRole(request.UserId, Role.Manager);
                    return Ok();
                default:
                    return BadRequest();
            }
        }
    }
}
