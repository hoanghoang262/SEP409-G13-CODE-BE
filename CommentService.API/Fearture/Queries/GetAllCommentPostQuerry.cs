using CommentService.API.Models;
using Contract.Service.Message;
using ForumService.API.Common.DTO;
using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetAllCommentPostQuerry : IRequest<IActionResult>
    {
        public int PostId { get; set; }

        public class GetAllCommentPostQuerryHandler : IRequestHandler<GetAllCommentPostQuerry, IActionResult>
        {
            private readonly GetUserInfoGrpcService _service;
            private readonly CommentContext _context;
            public GetAllCommentPostQuerryHandler(GetUserInfoGrpcService service, CommentContext context)
            {
                _service = service;
                _context = context;
            }
            public async Task<IActionResult> Handle(GetAllCommentPostQuerry request, CancellationToken cancellationToken)
            {
                var querry = await _context.Comments.Include(c => c.Replies).Where(c => c.ForumPostId != null && c.ForumPostId.Equals(request.PostId)).ToListAsync();
                if (querry == null)
                {
                    return new NotFoundObjectResult(Message.MSG22);
                }
                List<CommentDTO> post = new List<CommentDTO>();
                foreach (var c in querry)
                {
                    var id = c.UserId;
                    var userInfo = await _service.SendUserId((int)id);

                    List<ReplyDTO> replies = new List<ReplyDTO>();
                    foreach (var reply in c.Replies)
                    {
                        var replyUserInfo = await _service.SendUserId((int)reply.UserId);
                        replies.Add(new ReplyDTO
                        {
                            CommentId = reply.CommentId,
                            ReplyContent = reply.ReplyContent,
                            UserId = (int)reply.UserId,
                            Id = reply.Id,
                            UserName = replyUserInfo.Name,
                            UserPicture = replyUserInfo.Picture,
                            CreateDate = reply.CreateDate,
                        });
                    }

                    post.Add(new CommentDTO
                    {

                        UserName = userInfo.Name,
                        CommentContent = c.CommentContent,
                        Date = c.Date,
                        Picture = userInfo.Picture,
                        UserId = userInfo.Id,
                        Id = c.Id,
                        Replies = replies
                    });
                }

                return new OkObjectResult(post);
            }

        }
    }
}
