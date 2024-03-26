using MediatR;
using Microsoft.AspNetCore.Mvc;
using ForumService.API.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ForumService.API.Feature.Posts.Command
{
    public class UpdatePostCommand : IRequest<IActionResult>
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
    }

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, IActionResult>
    {
        private readonly ForumContext _context;

        public UpdatePostCommandHandler(ForumContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.PostId);

            if (post == null)
            {
                return new NotFoundResult();
            }

            post.Title = request.Title ?? post.Title;
            post.Description = request.Description ?? post.Description;
            post.PostContent = request.PostContent ?? post.PostContent;
            post.LastUpdate = DateTime.UtcNow;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return new OkObjectResult(post);
        }
    }
}
