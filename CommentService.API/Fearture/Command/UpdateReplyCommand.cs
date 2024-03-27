using CommentService.API.Models;
using Contract.Service.Message;
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
                if (string.IsNullOrEmpty(request.ReplyContent))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                var reply = await _context.Replies.FindAsync(request.ReplyId);
                if (reply == null)
                {
                    return new NotFoundObjectResult(Message.MSG38);
                }

                reply.ReplyContent = request.ReplyContent;
                await _context.SaveChangesAsync();

                return new OkObjectResult(Message.MSG16);
            }
        }
    }
}
