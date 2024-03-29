using Contract.SeedWork;
using Contract.Service.Message;
using ForumService.API.Common.DTO;
using ForumService.API.GrpcServices;
using ForumService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetAllPostByUserId : IRequest<IActionResult>
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int UserId { get; set; }

        public class GetAllPostByUserIdHandler : IRequestHandler<GetAllPostByUserId, IActionResult>
        {
            private readonly ForumContext _context;
            private readonly GetUserInfoService _service;

            public GetAllPostByUserIdHandler(ForumContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(GetAllPostByUserId request, CancellationToken cancellationToken)
            {
                var user = await _service.SendUserId(request.UserId);
                if (user.Id == 0)
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }

                var querry = await _context.Posts.Where(c => c.CreatedBy == user.Id).ToListAsync();
                if (querry == null)
                {
                    return new NotFoundObjectResult(Message.MSG22);
                }

                List<PostDTO> post = new List<PostDTO>();
                foreach (var c in querry)
                {
                    var id = c.CreatedBy;
                    var userInfo = await _service.SendUserId(id);
                    post.Add(new PostDTO
                    {
                        CreatedBy = c.CreatedBy,
                        UserName = userInfo.Name,
                        Description = c.Description,
                        LastUpdate = c.LastUpdate,
                        PostContent = c.PostContent,
                        Id = c.Id,
                        Title = c.Title,
                        Picture = userInfo.Picture,
                    });

                }
                var total = post.Count;
                var result = new PageList<PostDTO>(post, total, request.page, request.pageSize);

                return new OkObjectResult(result);
            }
        }
    }
}
