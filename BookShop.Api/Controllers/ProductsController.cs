using BookShop.Core.Mediatr.Product.Commands.Create;
using BookShop.Core.Mediatr.Product.Commands.Delete;
using BookShop.Core.Mediatr.Product.Commands.Update;
using BookShop.Core.Mediatr.Product.Queries.GetAll;
using BookShop.Core.Mediatr.Product.Queries.GetByAuthor;
using BookShop.Core.Mediatr.Product.Queries.GetByCategory;
using BookShop.Core.Mediatr.Product.Queries.GetById;
using BookShop.Core.Mediatr.Product.Queries.GetByName;
using BookShop.Core.Mediatr.Product.Queries.GetByPrice;
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
    public class ProductsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator)
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

        [HttpGet("/ByAuthor/{id}")]
        public async Task<IActionResult> GetByAuthor(int id)
        {
            IList<ProductModel> products = await mediator.Send(new GetByAuthorProduct.Query(id));
            return Ok(products);
        }

        [HttpGet("/ByCategory/{id}")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            IList<ProductModel> products = await mediator.Send(new GetByCategoryProduct.Query(id));
            return Ok(products);
        }

        [HttpGet("/ByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            IList<ProductModel> products = await mediator.Send(new GetByNameProduct.Query(name));
            return Ok(products);
        }

        [HttpGet("/ByPrice/{Min}/{Max}")]
        public async Task<IActionResult> GetByName(decimal min, decimal max)
        {
            IList<ProductModel> products = await mediator.Send(new GetByPriceProduct.Query(min, max));
            return Ok(products);
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
