using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Moderations
{
    public class GetModerationPostByIdQuerry : IRequest<IActionResult>
    {
        public int PostId { get; set; }
        public class GetModerationPostByIdQuerryHandler : IRequestHandler<GetModerationPostByIdQuerry, IActionResult>
        {
            private readonly Content_ModerationContext _context;
            private readonly GetUserInfoService service;

            public GetModerationPostByIdQuerryHandler(Content_ModerationContext context, GetUserInfoService _service)
            {
                _context = context;
                service = _service;
            }

            public async Task<IActionResult> Handle(GetModerationPostByIdQuerry request, CancellationToken cancellationToken)
            {
                // Check if post exists
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId);
                if (post == null)
                {
                    return new NotFoundObjectResult(post);
                }

                var user = await service.SendUserId((int)post.CreatedBy);
                if (user.Id == 0)
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }

                var postDTO = new PostDTO
                {
                    CreatedBy = post.CreatedBy,
                    Description = post.Description,
                    Id = post.Id,
                    LastUpdate = post.LastUpdate,
                    PostContent = post.PostContent,
                    Title = post.Title,
                    UserName = user.Name,
                    UserPicture = user.Picture
                };

                return new OkObjectResult(postDTO);
            }
        }
    }
}
