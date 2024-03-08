using CommentService.API.Models;
using MediatR;

namespace CommentService.API.Fearture.Command
{
    public class CreateCommentCommand : IRequest<Comment>
    {
        public int Id { get; set; }
        public int? LessonId { get; set; }
        public string? CommentContent { get; set; }
        public DateTime? Date { get; set; }
        public int? CourseId { get; set; }
        public int? ForumPostId { get; set; }
        public int UserId { get; set; }

        public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Comment>
        {
            private readonly CommentContext _context;
            public CreateCommentCommandHandler(CommentContext context)
            {
                _context = context;
            }
            public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = new Comment
                {
                    LessonId = request.LessonId,
                    CommentContent = request.CommentContent,
                    Date = request.Date ?? DateTime.Now, // Nếu không có ngày được chỉ định, sẽ sử dụng ngày hiện tại
                    CourseId = request.CourseId,
                    ForumPostId = request.ForumPostId,
                    UserId = request.UserId
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync(cancellationToken);

                return comment;

            }
        }

    }
}
