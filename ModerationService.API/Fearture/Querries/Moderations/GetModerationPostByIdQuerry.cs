using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Moderations
{
    public class GetModerationPostByIdQuerry : IRequest<ActionResult <PostDTO>>
    {
        public int PostId { get; set; }
        public class GetModerationPostByIdQuerryHandler : IRequestHandler<GetModerationPostByIdQuerry, ActionResult<PostDTO>>
        {
            private readonly Content_ModerationContext _context;
            private readonly GetUserInfoService service;

            public GetModerationPostByIdQuerryHandler(Content_ModerationContext context,GetUserInfoService _service)
            {
                _context = context;
                service= _service;  
            }

            public async Task<ActionResult<PostDTO>> Handle(GetModerationPostByIdQuerry request, CancellationToken cancellationToken)
            {
               
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId);
                var user = await service.SendUserId((int)post.CreatedBy);
                if (post == null)
                {
                    return new NoContentResult();
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

                return postDTO; 
            }
        }
    }
}
