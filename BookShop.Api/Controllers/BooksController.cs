using BookShop.Core.Mediatr.Book.Commands.Create;
using BookShop.Core.Mediatr.Book.Commands.Delete;
using BookShop.Core.Mediatr.Book.Commands.Update;
using BookShop.Core.Mediatr.Book.Queries;
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
    public class BooksController : ControllerBase
    {
        private readonly IMediator mediator;

        public BooksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<BookModel> products = await mediator.Send(new GetAllBookQuery());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            BookModel product = await mediator.Send(new GetByIdBookQuery(id));
            return Ok(product);
        }

        [HttpGet("/ByAuthor/{id}")]
        public async Task<IActionResult> GetByAuthor(int id)
        {
            IList<BookModel> products = await mediator.Send(new GetByAuthorBookQuery(id));
            return Ok(products);
        }

        [HttpGet("/ByCategory/{id}")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            IList<BookModel> products = await mediator.Send(new GetByCategoryBookQuery(id));
            return Ok(products);
        }

        [HttpGet("/ByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            IList<BookModel> products = await mediator.Send(new GetByNameBookQuery(name));
            return Ok(products);
        }

        [HttpGet("/ByPrice/{Min}/{Max}")]
        public async Task<IActionResult> GetByName(decimal min, decimal max)
        {
            IList<BookModel> products = await mediator.Send(new GetByPriceBookQuery(min, max));
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Add([FromBody] CreateBookCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Update([FromBody] UpdateBookCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteBookCommand(id));
            return Ok();
        }
    }
}
