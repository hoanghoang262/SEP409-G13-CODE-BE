using CommentService.API.Models;
using CourseService.API.Common.Mapping;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentService.API.Fearture.Command
{
    public class DeleteAllCommentCommand : IRequest<IActionResult>, IMapFrom<PostIdEvent>
    {
        public int PostId { get; set; }

        public class DeleteAllCommentCommandHandler : IRequestHandler<DeleteAllCommentCommand, IActionResult>
        {
            private readonly CommentContext _context;

            public DeleteAllCommentCommandHandler(CommentContext context)
            {

                _context = context;
            }

            public async Task<IActionResult> Handle(DeleteAllCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = await _context.Comments.Where(c => c.ForumPostId.Equals(request.PostId)).ToListAsync();
                if(comment == null) {
                    return new BadRequestObjectResult("Not found");
                }
                _context.Comments.RemoveRange(comment);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult("Delete succes");
            }
        }
    }
}
