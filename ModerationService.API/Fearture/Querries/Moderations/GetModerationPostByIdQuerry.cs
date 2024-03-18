using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Moderations
{
    public class GetModerationPostByIdQuerry : IRequest<ActionResult <Post>>
    {
        public int PostId { get; set; }
        public class GetModerationPostByIdQuerryHandler : IRequestHandler<GetModerationPostByIdQuerry, ActionResult<Post>>
        {
            private readonly Content_ModerationContext _context;

            public GetModerationPostByIdQuerryHandler(Content_ModerationContext context)
            {
                _context = context;
            }

            public async Task<ActionResult<Post>> Handle(GetModerationPostByIdQuerry request, CancellationToken cancellationToken)
            {
               
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId);
                if (post == null)
                {
                    return new NoContentResult();
                }

                return post; 
            }
        }
    }
}
