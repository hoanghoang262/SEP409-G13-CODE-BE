using Contract.Service.Message;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.EnrollmentFeature.Command
{
    public class CreateEnrollmentCourseCommand : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
    public class CreateEnrollmentCourseCommandHandler : IRequestHandler<CreateEnrollmentCourseCommand, IActionResult>
    {
        private readonly CourseContext _context;
        private readonly GetUserInfoService _service;

        public CreateEnrollmentCourseCommandHandler(CourseContext context, GetUserInfoService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<IActionResult> Handle(CreateEnrollmentCourseCommand request, CancellationToken cancellationToken)
        {
            // Check if the user exists
            var user = await _service.SendUserId(request.UserId);
            if (user.Id == 0)
            {
                return new NotFoundObjectResult(Message.MSG01);
            }

            // Check if the course exists
            var course = _context.Courses.FirstOrDefault(item => item.Id == request.CourseId);
            if (course == null)
            {
                return new NotFoundObjectResult(Message.MSG25);
            }
            var courseEnrolled = _context.Enrollments.FirstOrDefault(c => c.CourseId == request.CourseId && c.UserId == request.UserId);
             if(courseEnrolled !=null){
                return new NotFoundObjectResult("Bạn đã vào khóa học");
            }
           

            var enroll = new Enrollment
            {
                CourseId = request.CourseId,
                UserId = request.UserId,
            };
            _context.Enrollments.Add(enroll);
            await _context.SaveChangesAsync();

            return new OkObjectResult(enroll.Id);
        }
    }
}
