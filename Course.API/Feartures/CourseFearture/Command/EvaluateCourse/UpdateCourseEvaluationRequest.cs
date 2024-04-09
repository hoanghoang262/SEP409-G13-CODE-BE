using Contract.Service.Message;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.EvaluateCourse
{
    public class UpdateCourseEvaluationRequest : IRequest<IActionResult>
    {
        public int Id { get; set; }

        public double? Star { get; set; }
        public class UpdateCourseEvaluationHandler : IRequestHandler<UpdateCourseEvaluationRequest, IActionResult>
        {
            private readonly CourseContext _context;

            public UpdateCourseEvaluationHandler(CourseContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(UpdateCourseEvaluationRequest request, CancellationToken cancellationToken)
            {
                var evaluation = await _context.CourseEvaluations.FindAsync(request.Id);
                if (evaluation == null)
                {
                    return new BadRequestObjectResult(Message.MSG41);
                }

                if (request.Star < 0)
                {
                    return new BadRequestObjectResult(Message.MSG26);
                }

                evaluation.Star = request.Star ?? evaluation.Star;

                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(evaluation.Id);
            }
        }
    }
}
