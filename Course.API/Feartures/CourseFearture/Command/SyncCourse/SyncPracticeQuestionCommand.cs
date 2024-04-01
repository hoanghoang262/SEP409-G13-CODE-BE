using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncPracticeQuestionCommand : IRequest<IActionResult>, IMapFrom<PracticeQuestionEvent>
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }
        public string? CodeForm { get; set; }
        public string? TestCaseJava { get; set; }
        public string? TestCaseC { get; set; }
        public string? TestCaseCplus { get; set; }
        public class SyncCodeQuestionCommandHandler : IRequestHandler<SyncPracticeQuestionCommand, IActionResult>
        {
            private readonly CourseContext _context;
           

            public SyncCodeQuestionCommandHandler(CourseContext context)
            {
                _context = context;
               
            }
            public async Task<IActionResult> Handle(SyncPracticeQuestionCommand request, CancellationToken cancellationToken)
            {

                var existingCodeQuestion = await _context.PracticeQuestions.FindAsync(request.Id);
                if (existingCodeQuestion == null)
                {
                    var newCodeQuestion = new PracticeQuestion
                    {
                        Id = request.Id,
                        ChapterId = request.ChapterId,
                        Description = request.Description,
                        CodeForm = request.CodeForm,
                        TestCaseJava=request.TestCaseJava,
                        TestCaseC=request.TestCaseC,
                        TestCaseCplus=request.TestCaseCplus,

                    };
                    _context.PracticeQuestions.Add(newCodeQuestion);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    existingCodeQuestion.Id = request.Id;
                    existingCodeQuestion.ChapterId = request.ChapterId;
                    existingCodeQuestion.Description = request.Description;
                    existingCodeQuestion.CodeForm = request.CodeForm;
                    existingCodeQuestion.TestCaseJava = request.TestCaseJava;
                    existingCodeQuestion.TestCaseC = request.TestCaseC;
                    existingCodeQuestion.TestCaseCplus = request.TestCaseCplus;
                    await _context.SaveChangesAsync();

                }




                return new OkObjectResult("done");
            }
        }
    }
}
