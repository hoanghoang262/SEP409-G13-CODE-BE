using CommentService.API.Models;
using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentService.API.Fearture.Command
{
    public class CreateReplyCommand : IRequest<IActionResult>
    {
        public int CommentId { get; set; }
        public string ReplyContent { get; set; }
        public int UserId { get; set; }
    }


    public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, IActionResult>
    {
        private readonly CommentContext _context;

        public CreateReplyCommandHandler(CommentContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ReplyContent))
            {
                return new BadRequestObjectResult(Message.MSG11);
            }

            var comment = await _context.Comments
                    .Include(c => c.Replies)
                    .FirstOrDefaultAsync(c => c.Id == request.CommentId);

            if (comment == null)
            {
                return new NotFoundObjectResult(Message.MSG37);
            }

            var reply = new Reply
            {
                CommentId = request.CommentId,
                ReplyContent = request.ReplyContent,
                UserId = request.UserId,
                CreateDate = DateTime.UtcNow
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync(cancellationToken);

            return new OkObjectResult(reply.Id);
        }
    }
}
