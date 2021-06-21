using BookShop.Core.Mediatr.Photo.Commands.Create;
using BookShop.Core.Mediatr.Photo.Commands.Delete;
using BookShop.Core.Mediatr.Photo.Queries;
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
    public class PhotosController : ControllerBase
    {
        private readonly IMediator mediator;

        public PhotosController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<PhotoModel> photos = await mediator.Send(new GetAllPhotoQuery());
            return Ok(photos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            PhotoModel photo = await mediator.Send(new GetByIdPhotoQuery(id));
            return Ok(photo);
        }

        [HttpPost]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Add([FromBody] CreatePhotoCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeletePhotoCommand(id));
            return Ok();
        }
    }
}
