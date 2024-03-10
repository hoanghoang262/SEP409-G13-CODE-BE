using CommentService.API.Models;
using ForumService.API.Common.DTO;
using GrpcServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetAllCommentPostQuerry : IRequest<List<CommentDTO>>
    {
        public int PostId { get; set; }

        public class GetAllCommentPostQuerryHandler : IRequestHandler<GetAllCommentPostQuerry, List<CommentDTO>>
        {
            private readonly GetUserInfoGrpcService _service;
            private readonly CommentContext _context;
            public GetAllCommentPostQuerryHandler(GetUserInfoGrpcService service, CommentContext context)
            {
                _service = service;
                _context= context;
            }
            public async Task<List<CommentDTO>> Handle(GetAllCommentPostQuerry request, CancellationToken cancellationToken)
            {
                var querry = await _context.Comments.Include(c => c.Replies).Where(c => c.ForumPostId != null && c.ForumPostId.Equals(request.PostId)).ToListAsync();
                if (querry == null)
                {
                    return null;
                }
                List<CommentDTO> post = new List<CommentDTO>();
                foreach (var c in querry)
                {
                    var id = c.UserId;
                    var userInfo = await _service.SendUserId(id);

                    List<ReplyDTO> replies = new List<ReplyDTO>();
                    foreach (var reply in c.Replies)
                    {
                        var replyUserInfo = await _service.SendUserId(reply.UserId);
                        replies.Add(new ReplyDTO
                        {
                            CommentId = reply.CommentId,
                            ReplyContent = reply.ReplyContent,
                            UserId = reply.UserId,
                            Id = reply.Id,
                            UserName = replyUserInfo.Name 
                        });
                    }

                    post.Add(new CommentDTO
                    {
                        UserId = c.UserId,
                        UserName = userInfo.Name,
                        CommentContent = c.CommentContent,
                        Date = c.Date,
                        Picture = userInfo.Picture,
                        Id = userInfo.Id,
                        Replies = replies
                    });
                }

                return post;
            }

        }
    }
}
