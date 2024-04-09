using Contract.Service.Message;
using CourseService.API.Common.ModelDTO;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetProcessCourseQuerry : IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public class GetProcessCourseQuerryHandler : IRequestHandler<GetProcessCourseQuerry, IActionResult>
        {
            private readonly CourseContext _dbContext;
            private readonly GetUserInfoService _userInfoService;


            public GetProcessCourseQuerryHandler(CourseContext dbContext,GetUserInfoService getUserInfoService)
            {
                _dbContext = dbContext;
                _userInfoService= getUserInfoService;
            }

            public async Task<IActionResult> Handle(GetProcessCourseQuerry request, CancellationToken cancellationToken)
            {
                var user = await _userInfoService.SendUserId(request.UserId);
                if (user.Id == 0)
                {
                    return new BadRequestObjectResult(Message.MSG24);
                }
                var enrolledCourses = (from e in _dbContext.Enrollments
                             join c in _dbContext.Courses on e.CourseId equals c.Id
                             where e.UserId == request.UserId
                             select new
                             {
                                 Course = c,                       
                                 TotalLessons = _dbContext.Chapters
                                 .Where(ch => ch.CourseId == e.CourseId)
                                 .SelectMany(ch => ch.Lessons)
                                 .Count(),
                                 TotalPracticeQuestion = _dbContext.Chapters
                                 .Where(ch => ch.CourseId == e.CourseId)
                                 .SelectMany(ch => ch.PracticeQuestions)
                                 .Count(),
                                 TotalLastExam = _dbContext.Chapters
                                   .Where(ch => ch.CourseId == e.CourseId)
                                   .SelectMany(ch => ch.LastExams)
                                 .Count(),
                             CompletedPracticeQuestion = _dbContext.CompletedPracticeQuestions.Where(cl => cl.UserId == request.UserId).Count(),
                             CompletedLastExam = _dbContext.CompletedExams.Where(cl => cl.UserId == request.UserId).Count(),
                              CompletedLessons = _dbContext.CompleteLessons.Where(cl => cl.UserId == request.UserId).Count()    ,
                             }).ToList();

               

                var userProfile = new UserProfileDto
                {

                    EnrolledCourses = enrolledCourses.Select(ec => new CourseCompletionDto
                    {
                        CourseId = ec.Course.Id,
                        CourseName = ec.Course.Name,
                        CoursePicture = ec.Course.Picture,
                        CompletionPercentage = CalculateCompletionPercentage(ec.TotalLessons,ec.TotalPracticeQuestion
                        ,ec.TotalLastExam,ec.CompletedLessons,ec.CompletedPracticeQuestion,ec.CompletedLastExam),
                        IsDone= CalculateCompletionPercentage(ec.TotalLessons, ec.TotalPracticeQuestion
                        , ec.TotalLastExam, ec.CompletedLessons, ec.CompletedPracticeQuestion, ec.CompletedLastExam)<100?false:true
                    }).ToList()
                };
                return new OkObjectResult (userProfile);
            }
            private double CalculateCompletionPercentage(int totalLessons,int totalPractice, int TotalLastExam,
                int completedLessons,int completedPractice,int completedLastExam)
            {
                
                int total = totalLessons + totalPractice + TotalLastExam;
                int completed = completedLessons + completedPractice + completedLastExam;
                if (total == 0)
                    return 0;

                return ((double)completed / total) * 100;
            }

        }


      
    }
}
