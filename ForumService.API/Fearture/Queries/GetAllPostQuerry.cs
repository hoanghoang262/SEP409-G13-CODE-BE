using Contract.SeedWork;
using ForumService.API.Common.DTO;
using ForumService.API.GrpcServices;
using ForumService.API.Models;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetAllPostQuerry : IRequest<IActionResult>
    {
        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public string? PostTitle { get; set; }


        public class GetAllPostQuerryHandler : IRequestHandler<GetAllPostQuerry, IActionResult>
        {
            private readonly GetUserInfoService _service;
            private readonly ForumContext _context;
            public GetAllPostQuerryHandler(GetUserInfoService service,ForumContext context)
            {
                _service = service;
                _context= context;
            }
            public async Task<IActionResult> Handle(GetAllPostQuerry request, CancellationToken cancellationToken)
            {

               
                 var querry = await _context.Posts.ToListAsync();
                
                if(!string.IsNullOrEmpty(request.PostTitle))
                {
                    querry = await _context.Posts.Where(c => c.Title.Contains(request.PostTitle)).ToListAsync();

                }
                
                if (querry == null)
                {
                    return null;
                }
                var total = querry.Count();

                var items = querry
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

                List<PostDTO> post=new List<PostDTO>();  
                foreach(var c in items)
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
               

                var result = new PageList<PostDTO>(post, total, request.Page, request.PageSize);
                return new OkObjectResult(result);
            }
        }
    }
}
