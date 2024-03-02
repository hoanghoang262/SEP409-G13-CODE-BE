using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseService.API.Common.ModelDTO;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;

namespace CourseService.API.Feartures.Queries.CourseQueries
{
    public class GetCourseByUserIdQuerry : IRequest<IActionResult>
    {
        public int UserId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByUserIdQuerry, IActionResult>
        {
            private readonly Course_DeployContext _context;
            private readonly UserIdCourseGrpcService service;
            private readonly IMapper mapper;

            public GetCourseByUserHandler(Course_DeployContext context, UserIdCourseGrpcService userIdCourseGrpcService, IMapper _mapper)
            {
                _context = context;
                service = userIdCourseGrpcService;
                mapper = _mapper;

            }
            public async Task<IActionResult> Handle(GetCourseByUserIdQuerry request, CancellationToken cancellationToken)
            {

                var user = await service.SendUserId(request.UserId);

                var courses = _context.Courses.Where(c=>c.CreatedBy.Equals(user.Id)).ToList();
                        

                //var result = new
                //{
                //    UserName = user.Name,
                //    Courses = courses.Select(course => new
                //    {
                //        course.Id,
                //        course.Name,
                //        course.Description,
                //        course.Picture,
                //        course.Tag,
                //        Chapters = course.Chapters.Select(chapter => new
                //        {
                //            chapter.Id,
                //            chapter.Name,
                //            chapter.CourseId,
                //            chapter.Part,
                //            chapter.IsNew,
                //            Lessons = chapter.Lessons.Select(lesson => new
                //            {
                //                lesson.Id,
                //                lesson.Title,
                //                lesson.VideoUrl,
                //                lesson.ChapterId,
                //                lesson.Description,
                //                lesson.Duration,
                //                lesson.IsCompleted,
                //                Questions = lesson.Questions.Select(question => new
                //                {
                //                    question.Id,
                //                    question.VideoId,
                //                    question.ContentQuestion,

                //                    question.Time
                //                }).ToList()
                //            }).ToList()
                //        }).ToList()
                //    }).ToList()
                //};
                return new OkObjectResult(courses);
            }
        }
    }
}
