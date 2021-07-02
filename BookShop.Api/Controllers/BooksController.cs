using BookShop.Core.Abstract.Features.FileUploader;
using BookShop.Core.Mediatr.Book.Commands.Create;
using BookShop.Core.Mediatr.Book.Commands.Delete;
using BookShop.Core.Mediatr.Book.Commands.Update;
using BookShop.Core.Mediatr.Book.Queries;
using BookShop.Core.Models;
using BookShop.Features.FileUploader;
using BookShop.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly BaseFileUploader pdfFileUploader;

        public BooksController(IMediator mediator, IEnumerable<BaseFileUploader> baseFileUploaders)
        {
            this.mediator = mediator;
            pdfFileUploader = baseFileUploaders.FirstOrDefault(f => f.GetType() == typeof(PdfFileUploader));
        }

        [Authorize(Policy = RoleConstants.ModeratorName)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<BookModel> books = await mediator.Send(new GetAllBookQuery());
            return Ok(books);
        }

        [HttpGet("GetNewest")]
        public async Task<IActionResult> GetNewest()
        {
            IList<BookModel> books = await mediator.Send(new GetNewestBookQuery());
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            BookModel book = await mediator.Send(new GetByIdBookQuery(id));
            return Ok(book);
        }

        [HttpGet("ByAuthor/{id}")]
        public async Task<IActionResult> GetByAuthor(int id)
        {
            IList<BookModel> books = await mediator.Send(new GetByAuthorBookQuery(id));
            return Ok(books);
        }

        [HttpGet("ByCategory/{id}")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            IList<BookModel> books = await mediator.Send(new GetByCategoryBookQuery(id));
            return Ok(books);
        }

        [HttpGet("ByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            BookModel book = await mediator.Send(new GetByNameBookQuery(name));
            return Ok(book);
        }

        [HttpGet("ByPrice/{Min}/{Max}")]
        public async Task<IActionResult> GetByPrice(decimal min, decimal max)
        {
            IList<BookModel> books = await mediator.Send(new GetByPriceBookQuery(min, max));
            return Ok(books);
        }

        [HttpPost]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Add([FromBody] CreateBookCommand command)
        {
            BookModel book = await mediator.Send(command);
            return Ok(book);
        }

        [HttpPost("UploadFile/{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> UploadFile(int id)
        {
            await pdfFileUploader.UploadFile(Request.Body, id, (int)Request.ContentLength);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Update([FromBody] UpdateBookCommand command)
        {
            BookModel book = await mediator.Send(command);
            return Ok(book);
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
