using CourseService.API.Models;
using MediatR;

namespace CourseService.API.Feartures.EnrollmentFeature.Command
{
    public class CreateEnrollmentCourseCommand : IRequest<int>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }

    }
    public class CreateEnrollmentCourseCommandHandler : IRequestHandler<CreateEnrollmentCourseCommand, int>
    {
        private readonly CourseContext _context;

        public CreateEnrollmentCourseCommandHandler(CourseContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateEnrollmentCourseCommand request, CancellationToken cancellationToken)
        {
            var enroll = new Enrollment
            {
                CourseId = request.CourseId,
                UserId = request.UserId,
            };
            _context.Enrollments.Add(enroll);
            await _context.SaveChangesAsync();
            return enroll.Id;

        }
    }
}
