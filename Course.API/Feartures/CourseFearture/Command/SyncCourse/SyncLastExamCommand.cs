using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EventBus.Message.Event;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncLastExamCommand : IRequest<IActionResult>, IMapFrom<LastExamEvent>
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public int? PercentageCompleted { get; set; }
        public string? Name { get; set; }
        public int? Time { get; set; }
        public class SyncLastExamCommandHandler : IRequestHandler<SyncLastExamCommand, IActionResult>
        {
            private readonly CourseContext _context;
            public SyncLastExamCommandHandler(CourseContext context)
            {
                _context = context;

            }
            public async Task<IActionResult> Handle(SyncLastExamCommand request, CancellationToken cancellationToken)
            {
             

                var exAns = _context.LastExams.FirstOrDefault(x => x.Id.Equals(request.Id));
                if (exAns == null)
                {
                    var newExAns = new LastExam
                    {
                        Id = request.Id,
                        PercentageCompleted = request.PercentageCompleted,
                        ChapterId=request.ChapterId,
                        Name=request.Name,
                        Time=request.Time
                        

                    };

                    _context.LastExams.Add(newExAns);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {

                    exAns.PercentageCompleted = request.PercentageCompleted;
                    exAns.ChapterId = request.ChapterId;
                    exAns.Name = request.Name;
                    exAns.Time = request.Time;




                    await _context.SaveChangesAsync(cancellationToken);
                }
                return new OkObjectResult("done");
            }
        }
    }
}
