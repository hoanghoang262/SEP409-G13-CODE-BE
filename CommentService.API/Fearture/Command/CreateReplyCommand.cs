using CommentService.API.Models;
using MediatR;

namespace CommentService.API.Fearture.Command
{
    public class CreateReplyCommand : IRequest<int>
    {
        public int CommentId { get; set; }
        public string ReplyContent { get; set; }
        public int UserId { get; set; }
    }

    
    public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, int>
    {
        private readonly CommentContext _context;

        public CreateReplyCommandHandler(CommentContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
        {
            var reply = new Reply
            {
                CommentId = request.CommentId,
                ReplyContent = request.ReplyContent,
                UserId = request.UserId,
                CreateDate= DateTime.UtcNow
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync(cancellationToken);

            return reply.Id; 
        }
    }
}
