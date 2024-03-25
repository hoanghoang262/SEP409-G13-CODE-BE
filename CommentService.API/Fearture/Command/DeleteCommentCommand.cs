using CommentService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommentService.API.Feature.Command
{
    public class DeleteCommentCommand : IRequest<IActionResult>
    {
        public int CommentId { get; set; }

        public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, IActionResult>
        {
            private readonly CommentContext _context;

            public DeleteCommentCommandHandler(CommentContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = await _context.Comments.Include(c => c.Replies).FirstOrDefaultAsync(c => c.Id == request.CommentId);

                if (comment == null)
                {
                    return new NotFoundResult();
                }

                _context.Replies.RemoveRange(comment.Replies); 
                _context.Comments.Remove(comment); 
                await _context.SaveChangesAsync();

                return new OkResult();
            }
        }
    }
}
