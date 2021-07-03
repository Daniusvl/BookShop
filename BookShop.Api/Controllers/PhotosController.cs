using BookShop.Core.Abstract.Features.FileUploader;
using BookShop.Core.Mediatr.Photo.Commands.Create;
using BookShop.Core.Mediatr.Photo.Commands.Delete;
using BookShop.Core.Mediatr.Photo.Commands.Update;
using BookShop.Core.Mediatr.Photo.Queries;
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
    public class PhotosController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly BaseFileUploader pngFileUploader;

        public PhotosController(IMediator mediator, IEnumerable<BaseFileUploader> baseFileUploaders)
        {
            this.mediator = mediator;
            pngFileUploader = baseFileUploaders.FirstOrDefault(f => f.GetType() == typeof(PngFileUploader));
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
            PhotoModel photo = await mediator.Send(command);
            return Ok(photo);
        }

        [HttpPost("UploadFile/{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> UploadFile(int id)
        {
            await pngFileUploader.UploadFile(Request.Body, id, (int)Request.ContentLength);
            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Update(UpdatePhotoCommand command)
        {
            PhotoModel photo = await mediator.Send(command);
            return Ok(photo);
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
