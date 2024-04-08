using Contract.Service.Message;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.WishListFearture.Command
{
    public class CreateWishListCommand : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }

        public class CreateWishListCommandHandler : IRequestHandler<CreateWishListCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly GetUserInfoService _service;

            public CreateWishListCommandHandler(CourseContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }

            public async Task<IActionResult> Handle(CreateWishListCommand request, CancellationToken cancellationToken)
            {
                // Check if the user exists
                var user = await _service.SendUserId(request.UserId);
                if (user.Id == 0)
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }
                var wishlist = _context.Wishlists.FirstOrDefault(e => e.CourseId == request.CourseId && e.UserId == request.UserId);
                if(wishlist != null)
                {
                    return new OkObjectResult("Đã có trong wishlist ");

                }
               

                var wishlistItem = new Wishlist
                {
                    CourseId = request.CourseId,
                    UserId = request.UserId
                };

                _context.Wishlists.Add(wishlistItem);
                await _context.SaveChangesAsync();

                return new OkObjectResult(wishlistItem);
            }
        }
    }
}
