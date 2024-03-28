using Contract.Service.Message;
using ForumService.API.GrpcServices;
using ForumService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserGrpc;

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
            private readonly GetUserInfoService _service;

            public CreateForumCommandHandler(ForumContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(CreatAdminPostCommand request, CancellationToken cancellationToken)
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
