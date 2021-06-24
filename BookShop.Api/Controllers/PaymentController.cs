using BookShop.Core.Abstract;
using BookShop.Core.Abstract.Identity;
using BookShop.Core.Models.Payment;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IAuthenticationService authService;
        private readonly ILoggedInUser logged_in_user;

        public PaymentController(IAuthenticationService authService, ILoggedInUser logged_in_user)
        {
            this.authService = authService;
            this.logged_in_user = logged_in_user;
        }

        [HttpPost]
        public async Task<IActionResult> BuyBook([FromBody] BuyBookRequest buyBook)
        {
            bool result = await authService.AddOwnedProduct(logged_in_user.UserId, buyBook.BookId);

            return Ok(result);
        }
    }
}
