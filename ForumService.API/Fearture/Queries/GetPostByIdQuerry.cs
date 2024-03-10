using ForumService.API.Common.DTO;
using ForumService.API.Models;
using GrpcServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetPostByIdQuerry : IRequest<PostDTO>
    {
        public int PostId { get; set; }

        public class GetPostIdQuerryHandler : IRequestHandler<GetPostByIdQuerry, PostDTO>
        {
            private readonly ForumContext _context;
            private readonly GetUserPostGrpcService _service;
            public GetPostIdQuerryHandler(ForumContext context,GetUserPostGrpcService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<PostDTO> Handle(GetPostByIdQuerry request, CancellationToken cancellationToken)
            {
                
                var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id.Equals(request.PostId));
                var userInfo = await _service.SendUserId(post.CreatedBy);

                if (post == null)
                {
                    return null;
                }
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
                return postDTO;

            }
        }
    }
}
