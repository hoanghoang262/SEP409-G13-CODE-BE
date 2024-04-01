using Contract.Service.Message;
using ForumService.API.Common.DTO;
using ForumService.API.GrpcServices;
using ForumService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetPostByIdQuerry : IRequest<IActionResult>
    {
        public int PostId { get; set; }

        public class GetPostIdQuerryHandler : IRequestHandler<GetPostByIdQuerry, IActionResult>
        {
            private readonly ForumContext _context;
            private readonly GetUserInfoService _service;
            public GetPostIdQuerryHandler(ForumContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(GetPostByIdQuerry request, CancellationToken cancellationToken)
            {

                var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id.Equals(request.PostId));
                if (post == null)
                {
                    return new NotFoundObjectResult(post);
                }

                var userInfo = await _service.SendUserId(post.CreatedBy);
                var postDTO = new PostDTO
                {
                    CreatedBy = post.CreatedBy,
                    UserName = userInfo.Name,
                    Description = post.Description,
                    LastUpdate = post.LastUpdate,
                    PostContent = post.PostContent,
                    Id = post.Id,
                    Title = post.Title,
                    Picture = userInfo.Picture,
                };
                return new OkObjectResult(postDTO);

            }
        }
    }
}
