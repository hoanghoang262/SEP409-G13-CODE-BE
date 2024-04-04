using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.EnrollmentFeature.Command
{
    public class SyncEnrollCourseAfterPaymentCommand:IRequest<IActionResult>,IMapFrom<UserEnrollEvent>
    {
        public int? UserId { get; set; }

        public int? CourseId { get; set; }

        public class SyncEnrollCourseAfterPaymentCommandHandler : IRequestHandler<SyncEnrollCourseAfterPaymentCommand, IActionResult>
        {
            private readonly CourseContext _context;
            public SyncEnrollCourseAfterPaymentCommandHandler(CourseContext context)
            {
                _context=context;
            }
            public async Task<IActionResult> Handle(SyncEnrollCourseAfterPaymentCommand request, CancellationToken cancellationToken)
            {
                var emroll = new Enrollment
                {
                    CourseId = request.CourseId,
                    UserId = request.UserId
                };
                _context.Enrollments.Add(emroll);
               await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult("Ok");
                

            }
        }
    }
}
