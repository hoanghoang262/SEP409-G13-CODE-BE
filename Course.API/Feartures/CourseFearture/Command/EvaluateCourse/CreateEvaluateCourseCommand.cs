using Contract.Service.Message;
using CourseGRPC.Services;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.EvaluateCourse
{
    public class CreateEvaluateCourseCommand : IRequest<IActionResult>
    {
        public int CourseId {  get; set; }
        public int UserId { get; set; }

        public double Star { get; set; }
        public class CreateCourseEvaluationHandler : IRequestHandler<CreateEvaluateCourseCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly GetUserInfoService _service;
            private readonly CheckCourseIdServicesGrpc _checkCourseIdServicesGrpc;


            public CreateCourseEvaluationHandler(CourseContext context,GetUserInfoService service,CheckCourseIdServicesGrpc check)
            {
                _context = context;
                _service=service;
                _checkCourseIdServicesGrpc=check;
            }

            public async Task<IActionResult> Handle(CreateEvaluateCourseCommand request, CancellationToken cancellationToken)
            {
                var user = await _service.SendUserId(request.UserId);
                if (user.Id == 0)
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }
                var courseId = await _checkCourseIdServicesGrpc.SendCourseId(request.CourseId);
                if (courseId.IsCourseExist == 0)
                {
                    return new BadRequestObjectResult(Message.MSG25);

                }
                var evaluation = new CourseEvaluation
                {
                    UserId = request.UserId,
                    CourseId = request.CourseId,
                    Star = request.Star
                };

                _context.CourseEvaluations.Add(evaluation);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult( evaluation.Id);
            }
        }

    }
}
