using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetProcessCourseQuerry : IRequest<UserProfileDto>
    {
        public int UserId { get; set; }

        public class GetProcessCourseQuerryHandler : IRequestHandler<GetProcessCourseQuerry, UserProfileDto>
        {
            private readonly Course_DeployContext _dbContext;

            public GetProcessCourseQuerryHandler(Course_DeployContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<UserProfileDto> Handle(GetProcessCourseQuerry request, CancellationToken cancellationToken)
            {


                var enrolledCourses = await _dbContext.Enrollments
                    .Where(e => e.UserId == request.UserId)
                    .Select(e => new
                    {
                        Course = e.Course,
                        CompletedLessons = _dbContext.CompleteLessons
                            .Count(cl => cl.UserId == request.UserId),
                        TotalLessons = _dbContext.Chapters
                        .Where(ch => ch.CourseId == e.CourseId)
                        .SelectMany(ch => ch.Lessons)
                        .Count()
                    }).ToListAsync();



                var userProfile = new UserProfileDto
                {
                   
                    EnrolledCourses = enrolledCourses.Select(ec => new CourseCompletionDto
                    {
                        CourseId=ec.Course.Id,
                        CourseName = ec.Course.Name,
                        CoursePicture=ec.Course.Picture,
                        CompletionPercentage = CalculateCompletionPercentage(ec.TotalLessons, ec.CompletedLessons)
                    }).ToList()
                };

                return userProfile;
            }

            private double CalculateCompletionPercentage(int totalLessons, int completedLessons)
            {
                if (totalLessons == 0)
                    return 0;

                return ((double)completedLessons / totalLessons) * 100;
            }

        }


      
    }
}
