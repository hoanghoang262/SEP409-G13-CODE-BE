using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreatePostCommand : IRequest<IActionResult>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }

        public class CreateForumCommandHandler : IRequestHandler<CreatePostCommand, IActionResult>
        {
            private readonly Content_ModerationContext _context;
            private readonly GetUserInfoService _service;

            public CreateForumCommandHandler(Content_ModerationContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                // Validate request
                if (string.IsNullOrEmpty(request.Title)
                    || string.IsNullOrEmpty(request.Description)
                    || string.IsNullOrEmpty(request.PostContent))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                // Check if user exists
                var user = await _service.SendUserId(request.CreatedBy);
                if (user.Id == 0)
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }

                // Validate length
                if (request.Title.Length > 256)
                {
                    return new BadRequestObjectResult(Message.MSG27);
                }

                var post = new Post
                {
                    Title = request.Title,
                    Description = request.Description,
                    CreatedBy = request.CreatedBy,
                    LastUpdate = DateTime.Now,
                    PostContent = request.PostContent,
                };
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                var moder = new Moderation
                {
                    ChangeType = "Add",
                    ApprovedContent = "Phê duyệt bài đăng",
                    CreatedBy = request.CreatedBy,
                    CreatedAt = DateTime.Now,
                    Status = "Pending",
                    PostId = post.Id,
                    PostTitle = post.Title,
                };

                _context.Moderations.Add(moder);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(post.Id);
            }
        }

    }
}
