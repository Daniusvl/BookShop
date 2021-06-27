using BookShop.Core.Mediatr.Category.Commands.Create;
using BookShop.Core.Mediatr.Category.Commands.Delete;
using BookShop.Core.Mediatr.Category.Commands.Update;
using BookShop.Core.Mediatr.Category.Queries;
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
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<CategoryModel> categories = await mediator.Send(new GetAllCategoryQuery());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            CategoryModel category = await mediator.Send(new GetByIdCategoryQuery(id));
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Add([FromBody] CreateCategoryCommand command)
        {
            CategoryModel category = await mediator.Send(command);
            return Ok(category);
        }

        [HttpPut]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
        {
            CategoryModel category = await mediator.Send(command);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = RoleConstants.ModeratorName)]
        public async Task<IActionResult> Delete(int id)
        {
            await mediator.Send(new DeleteCategoryCommand(id));
            return Ok();
        }
    }
}
