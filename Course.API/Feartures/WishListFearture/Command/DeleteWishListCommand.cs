using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.WishListFearture.Command
{
    public class DeleteWishListCommand : IRequest<IActionResult>
    {
        public int WishlistId { get; set; }

        public class RemoveFromWishlistCommandHandler : IRequestHandler<DeleteWishListCommand, IActionResult>
        {
            private readonly CourseContext _context;

            public RemoveFromWishlistCommandHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(DeleteWishListCommand request, CancellationToken cancellationToken)
            {
                var wishlistItem = await _context.Wishlists.FindAsync(request.WishlistId);

                if (wishlistItem == null)
                {
                    return new BadRequestObjectResult(Message.MSG40);
                }

                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();

                return new OkObjectResult(Message.MSG16);
            }
        }
    }
}
