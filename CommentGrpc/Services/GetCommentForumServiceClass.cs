using CommentGrpc.Models;
using CommentGrpc.Protos;
using Grpc.Core;

namespace CommentGrpc.Services
{
    public class GetCommentForumServiceClass : CommentForumsService.CommentForumsServiceBase
    {
        private readonly CommentContext _context;
        public GetCommentForumServiceClass(CommentContext context) { 
                 _context=context;
        }

        public override Task<GetCommentForumsResponse> GetCommentForums(GetCommentForumsRequest request, ServerCallContext context)
        {
            var comments = _context.Comments.Where(e => e.ForumPostId == request.ForumId).ToList();



            if (comments == null)
            {
                return Task.FromResult<GetCommentForumsResponse>(null);
            }
            var response = new GetCommentForumsResponse();

            foreach (var comment in comments)
            {
                response.Comment.Add(new Comments
                {
                    UserId = comment.UserId,
                    UserName = comment.UserName,
                    Content = comment.CommentContent,
                    Date = comment.Date.ToString()
                }) ;
            }





            return Task.FromResult(response);
        }
    }
}
