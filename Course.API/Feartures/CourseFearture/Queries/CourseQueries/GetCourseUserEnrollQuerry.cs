using Contract.Service.Message;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetCourseUserEnrollQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
    public class GetCourseUserEnrollQuerryHandler : IRequestHandler<GetCourseUserEnrollQuerry, IActionResult>
    {
        private readonly CourseContext _context;
        private readonly GetUserInfoService service;

        public GetCourseUserEnrollQuerryHandler(CourseContext context, GetUserInfoService service)
        {
            _context = context;
            this.service = service;
        }
        public async Task<IActionResult> Handle(GetCourseUserEnrollQuerry request, CancellationToken cancellationToken)
        {
            var courses = await (from enrollment in _context.Enrollments
                                 join course in _context.Courses on enrollment.CourseId equals course.Id
                                 where enrollment.CourseId == request.CourseId && enrollment.UserId == request.UserId
                                 select new { Enrollment = enrollment, Course = course })
                                  .FirstOrDefaultAsync();
            if (courses == null)
            {
                return new NotFoundObjectResult(courses);
            }

            var user = await service.SendUserId(courses.Course.CreatedBy);
            if (user.Id == 0)
            {
                return new NotFoundObjectResult(courses);
            }

            var result = new
            {
                courses.Course.Id,
                courses.Course.Name,
                courses.Course.Description,
                courses.Course.Picture,
                courses.Course.Tag,
                courses.Course.CreatedBy,
                courses.Course.CreatedAt,
                Created_Name = user.Name,
                Avatar = user.Picture,
            };

            return new OkObjectResult(result);
        }
    }
}
