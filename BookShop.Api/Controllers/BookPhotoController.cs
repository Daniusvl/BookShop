using BookShop.Core.Mediatr.BookPhoto.Commands.Create;
using BookShop.Core.Mediatr.BookPhoto.Commands.Delete;
using BookShop.Core.Mediatr.BookPhoto.Queries.GetAll;
using BookShop.Core.Mediatr.BookPhoto.Queries.GetById;
using BookShop.Core.Models;
using BookShop.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookPhotoController : ControllerBase
    {
        private readonly IMediator mediator;

        public BookPhotoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<BookPhotoModel> photos = await mediator.Send(new GetAllBookPhoto.Query());
            return Ok(photos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            BookPhotoModel photo = await mediator.Send(new GetByIdBookPhoto.Query(id));
            return Ok(photo);
        }

        [HttpPost]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Add([FromBody] CreateBookPhoto.Command command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteBookPhoto.Command(id));
            return Ok();
        }
    }
}
