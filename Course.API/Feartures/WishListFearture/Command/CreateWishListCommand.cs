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

            public CreateWishListCommandHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(CreateWishListCommand request, CancellationToken cancellationToken)
            {
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
