using CommentService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.API.Fearture.Command
{
    public class UpdateReplyCommand : IRequest<IActionResult>
    {
        public int ReplyId { get; set; }
        public string ReplyContent { get; set; }

        public class UpdateReplyCommandHandler : IRequestHandler<UpdateReplyCommand, IActionResult>
        {
            private readonly CommentContext _context;

            public UpdateReplyCommandHandler(CommentContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(UpdateReplyCommand request, CancellationToken cancellationToken)
            {
                var reply = await _context.Replies.FindAsync(request.ReplyId);

                if (reply == null)
                {
                    return new NotFoundResult();
                }

                reply.ReplyContent = request.ReplyContent;

                await _context.SaveChangesAsync();

                return new OkResult();
            }
        }
    }
}
