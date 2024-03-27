using CommentService.API.Models;
using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.API.Fearture.Command
{
    public class DeleteReplyCommand : IRequest<IActionResult>
    {
        public int ReplyId { get; set; }

        public class DeleteReplyCommandHandler : IRequestHandler<DeleteReplyCommand, IActionResult>
        {
            private readonly CommentContext _context;

            public DeleteReplyCommandHandler(CommentContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(DeleteReplyCommand request, CancellationToken cancellationToken)
            {
                var reply = await _context.Replies.FindAsync(request.ReplyId);

                if (reply == null)
                {
                    return new NotFoundObjectResult(Message.MSG38);
                }

                _context.Replies.Remove(reply);
                await _context.SaveChangesAsync();

                return new OkObjectResult(Message.MSG16);
            }
        }
    }
}
