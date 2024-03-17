using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Forum
{
    public class UpdatePostCommand : IRequest<ActionResult<Post>>
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }

        public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ActionResult<Post>>
        {
            private readonly Content_ModerationContext _context;
            public UpdatePostCommandHandler(Content_ModerationContext context)
            {
                _context=context;
            }
            public async Task<ActionResult<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var post =await _context.Posts.FirstOrDefaultAsync(x=>x.Id.Equals(request.Id));

                if (post == null)
                {
                    return new BadRequestObjectResult("Not found");
                }

                post.Title = request.Title;
                post.PostContent = request.PostContent;
                post.Description = request.Description;
                post.LastUpdate = DateTime.Now;
                post.CreatedBy = request.CreatedBy;

                return new OkObjectResult(post);

            }
        }
    }
}
