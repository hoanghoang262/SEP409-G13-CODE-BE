using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;
using CourseService.API.GrpcServices;
using Contract.Service.Message;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetCourseByCourseIdQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByCourseIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly GetUserInfoService service;
            private readonly IMapper mapper;

            public GetCourseByUserHandler(CourseContext context, GetUserInfoService userIdCourseGrpcService, IMapper _mapper)
            {
                _context = context;
                service = userIdCourseGrpcService;
                mapper = _mapper;

            }
            public async Task<IActionResult> Handle(GetCourseByCourseIdQuerry request, CancellationToken cancellationToken)
            {
                var courses = await _context.Courses.Include(c=>c.Chapters).FirstOrDefaultAsync(c => c.Id.Equals(request.CourseId));

                if (courses == null)
                {
                    return new NotFoundObjectResult(Message.MSG22);
                }

                courses.Chapters.OrderBy(c => c.Part);

                var user = await service.SendUserId(courses.CreatedBy);

                var result = new
                {
                    courses.Id,
                    courses.Name,
                    courses.Description,
                    courses.Picture,
                    courses.Tag,
                    courses.CreatedBy,
                    courses.CreatedAt,
                    Created_Name = user.Name,
                    Avatar = user.Picture,
                    Chapters = courses.Chapters.Select(chapter => new
                    {
                        chapter.Id,
                        chapter.Name,
                        chapter.CourseId,
                        chapter.Part,
                        chapter.IsNew,
                    }).ToList()
                };

                return new OkObjectResult(result);
            }

        }
    }
}