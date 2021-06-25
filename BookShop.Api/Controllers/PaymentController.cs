using BookShop.Core.Abstract;
using BookShop.Core.Abstract.Identity;
using BookShop.Core.Models.Payment;
using BookShop.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = RoleConstants.DefaultUserName)]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILoggedInUser logged_in_user;

        public PaymentController(IUserService userService, ILoggedInUser logged_in_user)
        {
            this.userService = userService;
            this.logged_in_user = logged_in_user;
        }

        [HttpPost("/BuyBook")]
        public async Task<IActionResult> BuyBook([FromBody] BuyBookRequest buyBook)
        {
            await userService.AddOwnedProduct(logged_in_user.UserId, buyBook.BookId);

            return Ok();
        }
    }
}
