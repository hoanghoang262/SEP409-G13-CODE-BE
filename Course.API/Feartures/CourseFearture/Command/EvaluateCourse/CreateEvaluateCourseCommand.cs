using CourseService.API.Models;
using MediatR;

namespace CourseService.API.Feartures.CourseFearture.Command.EvaluateCourse
{
    public class CreateEvaluateCourseCommand : IRequest<int>
    {
        public int CourseId {  get; set; }
        public int UserId { get; set; }

        public double Star { get; set; }
        public class CreateCourseEvaluationHandler : IRequestHandler<CreateEvaluateCourseCommand, int>
        {
            private readonly CourseContext _context;

            public CreateCourseEvaluationHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateEvaluateCourseCommand request, CancellationToken cancellationToken)
            {
                var evaluation = new CourseEvaluation
                {
                    UserId = request.UserId,
                    CourseId = request.CourseId,
                    Star = request.Star
                };

                _context.CourseEvaluations.Add(evaluation);
                await _context.SaveChangesAsync(cancellationToken);

                return evaluation.Id;
            }
        }

    }
}
