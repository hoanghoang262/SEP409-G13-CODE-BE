using GrpcServices;
using CourseService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseService.API.Common.ModelDTO;
using Microsoft.EntityFrameworkCore;

namespace API.Feartures.CourseFearture.Queries
{
    public class GetCourseByUser : IRequest<IActionResult>
    {
        public int UserId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByUser, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly UserIdCourseGrpcService service;
            private readonly IMapper mapper;

            public GetCourseByUserHandler(CourseContext context, UserIdCourseGrpcService userIdCourseGrpcService, IMapper _mapper)
            {
                     _context=context;
                     service=userIdCourseGrpcService;
                mapper = _mapper;
                
            }
            public async Task<IActionResult> Handle(GetCourseByUser request, CancellationToken cancellationToken)
            {
                var user= await service.SendUserId(request.UserId);

                var courses = _context.Courses
                            .Include(course => course.Chapters)
                             .ThenInclude(chapter => chapter.Lessons)
                          .ThenInclude(lesson => lesson.Questions)
     .Where(course => course.UserId == request.UserId) // Lọc theo UserId nếu cần
     .ToList();

                var result = new
                {
                    UserName = user.Name,
                    Courses = courses.Select(course => new
                    {
                        Id = course.Id,
                        Name = course.Name,
                        Description = course.Description,
                        Picture = course.Picture,
                        Tag = course.Tag,
                        UserId = course.UserId,
                        Chapters = course.Chapters.Select(chapter => new
                        {
                            Id = chapter.Id,
                            Name = chapter.Name,
                            CourseId = chapter.CourseId,
                            Part = chapter.Part,
                            IsNew = chapter.IsNew,
                            Lessons = chapter.Lessons.Select(lesson => new
                            {
                                Id = lesson.Id,
                                Title = lesson.Title,
                                VideoUrl = lesson.VideoUrl,
                                ChapterId = lesson.ChapterId,
                                Description = lesson.Description,
                                Duration = lesson.Duration,
                                Questions = lesson.Questions.Select(question => new
                                {
                                    Id = question.Id,
                                    VideoId = question.VideoId,
                                    ContentQuestion = question.ContentQuestion,
                                    AnswerA = question.AnswerA,
                                    AnswerB = question.AnswerB,
                                    AnswerC = question.AnswerC,
                                    AnswerD = question.AnswerD,
                                    CorrectAnswer = question.CorrectAnswer,
                                    Time = question.Time
                                }).ToList() 
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };

                return new OkObjectResult(result);
            }
        }
    }
}
