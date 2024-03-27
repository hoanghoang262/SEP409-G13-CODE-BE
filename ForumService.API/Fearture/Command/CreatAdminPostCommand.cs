using ForumService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForumService.API.Fearture.Command
{
    public class CreatAdminPostCommand : IRequest<IActionResult>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }

        public class CreateForumCommandHandler : IRequestHandler<CreatAdminPostCommand, IActionResult>
        {
            private readonly ForumContext _context;

            public CreateForumCommandHandler(ForumContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(CreatAdminPostCommand request, CancellationToken cancellationToken)
            {

                var post = new Post
                {
                    Title = request.Title,
                    Description = request.Description,
                    CreatedBy = request.CreatedBy,
                    PostContent = request.PostContent,
                    LastUpdate = DateTime.Now,
                };
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return new OkObjectResult(post.Id);
            }
        }

    }
}
