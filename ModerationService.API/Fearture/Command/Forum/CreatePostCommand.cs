using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreatePostCommand : IRequest<int>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }

        public class CreateForumCommandHandler : IRequestHandler<CreatePostCommand, int>
        {
            private readonly Content_ModerationContext _context;

            public CreateForumCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                var post = new Post
                {
                    Title = request.Title,
                    Description = request.Description,
                    CreatedBy = request.CreatedBy,
                    LastUpdate = request.LastUpdate,
                };
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                var moder = new Moderation
                {
                    ChangeType = "Add",
                    ApprovedContent="Phê duyệt bài đăng",
                    CreatedBy=request.CreatedBy,
                    CreatedAt=request.LastUpdate,
                    PostId=post.Id,
                    PostTitle=post.Title,   
                };
                _context.Moderations.Add(moder);
                await _context.SaveChangesAsync(cancellationToken);

                return post.Id;
            }
        }

    }
}
