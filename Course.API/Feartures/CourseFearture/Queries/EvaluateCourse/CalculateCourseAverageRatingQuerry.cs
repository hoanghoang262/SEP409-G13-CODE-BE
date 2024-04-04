using CourseService.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.EvaluateCourse
{
    public class CalculateCourseAverageRatingQuerry : IRequest<double>
    {
        public int CourseId { get; set; }
        public class CalculateCourseAverageRatingHandler : IRequestHandler<CalculateCourseAverageRatingQuerry, double>
        {
            private readonly CourseContext _context;

            public CalculateCourseAverageRatingHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<double> Handle(CalculateCourseAverageRatingQuerry request, CancellationToken cancellationToken)
            {
                var averageRating = await _context.CourseEvaluations
                    .Where(e => e.CourseId == request.CourseId && e.Star != null)
                    .AverageAsync(e => e.Star.Value, cancellationToken);

                return Math.Round(averageRating, 2);
            }
        }
    }
}
