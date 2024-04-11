using Contract.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Post
{
    public class GetPostByUserIdQuerry : IRequest<IActionResult>
    {
        public int UserId { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? PostTitle { get; set; }
        public class GetPostByUserIdQuerryHandler : IRequestHandler<GetPostByUserIdQuerry, IActionResult>
        {
            private readonly Content_ModerationContext _context;
            private readonly GetUserInfoService _service ;
            public GetPostByUserIdQuerryHandler(Content_ModerationContext context,GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(GetPostByUserIdQuerry request, CancellationToken cancellationToken)
            {
                var querry = await _context.Posts.ToListAsync();

                if (!string.IsNullOrEmpty(request.PostTitle))
                {
                    querry = await _context.Posts.Where(c => c.Title.Contains(request.PostTitle) && c.CreatedBy.Equals(request.UserId)).ToListAsync();
                }
                if (string.IsNullOrEmpty(request.PostTitle))
                {
                    querry = await _context.Posts.Where(c => c.CreatedBy.Equals(request.UserId)).ToListAsync();
                }

                var total = querry.Count();

                var items = querry
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .OrderByDescending(c => c.Id)
                .ToList();

                List<PostDTO> post = new List<PostDTO>();
                foreach (var c in items)
                {
                    var id = c.CreatedBy;
                    var userInfo = await _service.SendUserId((int)id);
                    post.Add(new PostDTO
                    {
                        CreatedBy = c.CreatedBy,
                        UserName = userInfo.Name,
                        Description = c.Description,
                        LastUpdate = c.LastUpdate,
                        PostContent = c.PostContent,
                        Id = c.Id,
                        Title = c.Title,
                        UserPicture = userInfo.Picture,
                    });
                }


                var result = new PageList<PostDTO>(post, total, request.Page, request.PageSize);
                return new OkObjectResult(result);
            }
        }
    }
}
