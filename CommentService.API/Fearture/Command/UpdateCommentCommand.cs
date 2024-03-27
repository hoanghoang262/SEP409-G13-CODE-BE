using CommentService.API.Models;
using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentService.API.Fearture.Command
{
    public class UpdateCommentCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public string? CommentContent { get; set; }
        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, IActionResult>
        {
            private readonly CommentContext _context;

            public UpdateCommentCommandHandler(CommentContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.CommentContent))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (comment == null)
                {
                    return new NotFoundObjectResult(Message.MSG37);
                }

                // Update comment properties
                if (request.CommentContent != null)
                {
                    comment.CommentContent = request.CommentContent;
                }
                if (request.Date != null)
                {
                    comment.Date = DateTime.Now;
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(comment);
            }
        }
    }
}
