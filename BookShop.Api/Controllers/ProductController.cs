using BookShop.Core.Mediatr.Product.Commands.Create;
using BookShop.Core.Mediatr.Product.Commands.Delete;
using BookShop.Core.Mediatr.Product.Commands.Update;
using BookShop.Core.Mediatr.Product.Queries.GetAll;
using BookShop.Core.Mediatr.Product.Queries.GetById;
using BookShop.Core.Models;
using BookShop.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<ProductModel> products = await mediator.Send(new GetAllProduct.Query());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            ProductModel product = await mediator.Send(new GetByIdProduct.Query(id));
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Add([FromBody] CreateProduct.Command command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Update([FromBody] UpdateProduct.Command command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteProduct.Command(id));
            return Ok();
        }
    }
}
