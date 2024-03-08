using CommentService.API.Models;
using ForumService.API.Common.DTO;
using GrpcServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetAllCommentCourseQuerry : IRequest<List<CommentDTO>>
    {


        public class GetAllCommentCourseQuerryHandler : IRequestHandler<GetAllCommentCourseQuerry, List<CommentDTO>>
        {
            private readonly GetUserInfoGrpcService _service;
            private readonly CommentContext _context;
            public GetAllCommentCourseQuerryHandler(GetUserInfoGrpcService service, CommentContext context)
            {
                _service = service;
                _context = context;
            }
            public async Task<List<CommentDTO>> Handle(GetAllCommentCourseQuerry request, CancellationToken cancellationToken)
            {

                var querry = await _context.Comments.Where(c => c.CourseId != null).ToListAsync();
                if(querry == null)
                {
                    return null;
                }
                List<CommentDTO> post = new List<CommentDTO>();
                foreach (var c in querry)
                {
                    var id = c.UserId;
                    var userInfo = await _service.SendUserId(id);
                    post.Add(new CommentDTO
                    {
                        UserId = c.UserId,
                        UserName = userInfo.Name,
                        CommentContent = c.CommentContent,
                        Date = c.Date,
                        Picture = userInfo.Picture,
                        Id = userInfo.Id,
                    });

                }


                return post;
            }
        }
    }
}
