using ForumService.API.Common.DTO;
using ForumService.API.Models;
using GrpcServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetAllPostQuerry : IRequest<List<PostDTO>>
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 5;


        public class GetAllPostQuerryHandler : IRequestHandler<GetAllPostQuerry, List<PostDTO>>
        {
            private readonly GetUserPostGrpcService _service;
            private readonly ForumContext _context;
            public GetAllPostQuerryHandler(GetUserPostGrpcService service,ForumContext context)
            {
                _service = service;
                _context= context;
            }
            public async Task<List<PostDTO>> Handle(GetAllPostQuerry request, CancellationToken cancellationToken)
            {
                
                var querry= await _context.Posts.ToListAsync();
                if (querry == null)
                {
                    return null;
                }
                List<PostDTO> post=new List<PostDTO>();  
                foreach(var c in querry)
                {
                    var id = c.CreatedBy;
                    var userInfo = await _service.SendUserId(id);
                    post.Add(new PostDTO
                    {
                        CreatedBy = c.CreatedBy,
                        UserName=userInfo.Name,
                        Description=c.Description,
                        LastUpdate=c.LastUpdate,
                        PostContent=c.PostContent,
                        Id=c.Id,
                        Title=c.Title,
                        Picture=userInfo.Picture,
                    });
                    
                }
               

               return post;
            }
        }
    }
}
