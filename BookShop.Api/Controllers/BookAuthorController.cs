using BookShop.Core.Mediatr.BookAuthor.Commands.Create;
using BookShop.Core.Mediatr.BookAuthor.Commands.Delete;
using BookShop.Core.Mediatr.BookAuthor.Commands.Update;
using BookShop.Core.Mediatr.BookAuthor.Queries.GetAll;
using BookShop.Core.Mediatr.BookAuthor.Queries.GetById;
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
    public class BookAuthorController : ControllerBase
    {
        private readonly IMediator mediator;

        public BookAuthorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<BookAuthorModel> authors = await mediator.Send(new GetAllBookAuthor.Query());
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            BookAuthorModel author = await mediator.Send(new GetByIdBookAuthor.Query(id));
            return Ok(author);
        }

        [HttpPost]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Add([FromBody] CreateBookAuthor.Command command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Update([FromBody] UpdateBookAuthor.Command command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteBookAuthor.Command(id));
            return Ok();
        }
    }
}
