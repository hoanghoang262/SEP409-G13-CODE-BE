using CourseService.API.Models;
using MediatR;

namespace CourseService.API.Feartures.CourseFearture.Command.EvaluateCourse
{
    public class UpdateCourseEvaluationRequest : IRequest<int>
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CourseId { get; set; }
        public double? Star { get; set; }
        public class UpdateCourseEvaluationHandler : IRequestHandler<UpdateCourseEvaluationRequest, int>
        {
            private readonly CourseContext _context;

            public UpdateCourseEvaluationHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(UpdateCourseEvaluationRequest request, CancellationToken cancellationToken)
            {
                var evaluation = await _context.CourseEvaluations.FindAsync(request.Id);

              

                evaluation.UserId = request.UserId ?? evaluation.UserId;
                evaluation.CourseId = request.CourseId ?? evaluation.CourseId;
                evaluation.Star = request.Star ?? evaluation.Star;

                await _context.SaveChangesAsync(cancellationToken);

                return evaluation.Id;
            }
        }
    }

  
}
