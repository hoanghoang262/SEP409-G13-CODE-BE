using CourseService.API.Feartures.WishListFearture.Command;
using CourseService.API.Feartures.WishListFearture.Querries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WishListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishListByUserId([FromQuery] GetWishListByUserIdQuerry query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] CreateWishListCommand command)
        {
            var wishlistItemId = await _mediator.Send(command);
            return Ok(wishlistItemId);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromWishlist([FromBody] DeleteWishListCommand command)
        {
            var success = await _mediator.Send(command);
            return Ok(success);
        }
    }
}
