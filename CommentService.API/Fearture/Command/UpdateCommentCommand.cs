using CommentService.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommentService.API.Fearture.Command
{
    public class UpdateCommentCommand : IRequest<Comment>
    {
        public int Id { get; set; }
        public string? CommentContent { get; set; }
        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Comment>
        {
            private readonly CommentContext _context;

            public UpdateCommentCommandHandler(CommentContext context)
            {
                _context = context;
            }

            public async Task<Comment> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (comment == null)
                {
                    throw new Exception("Comment not found.");
                }

                // Update comment properties
                if (request.CommentContent != null)
                {
                    comment.CommentContent = request.CommentContent;
                }
                if (request.Date != null)
                {
                    comment.Date = request.Date.Value;
                }
               

                await _context.SaveChangesAsync(cancellationToken);

                return comment;
            }
        }
    }
}
