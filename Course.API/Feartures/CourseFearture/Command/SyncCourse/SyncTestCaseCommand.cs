using CourseService.API.Common.Mapping;
using CourseService.API.Models;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Command.SyncCourse
{
    public class SyncTestCaseCommand : IRequest<IActionResult>, IMapFrom<TestCaseEvent>
    {
        public int Id { get; set; }
        public int? InputTypeInt { get; set; }
        public string? InputTypeString { get; set; }
        public int? ExpectedResultInt { get; set; }
        public int? CodeQuestionId { get; set; }
        public string? ExpectedResultString { get; set; }
        public bool? InputTypeBoolean { get; set; }
        public bool? ExpectedResultBoolean { get; set; }
        public string? InputTypeArrayInt { get; set; }
        public string? InputTypeArrayString { get; set; }
        public class asyncQuestionCommandHandler : IRequestHandler<SyncTestCaseCommand, IActionResult>
        {
            private readonly Course_DeployContext _context;
            

            public asyncQuestionCommandHandler(Course_DeployContext context)
            {
                _context = context;
                
            }
            public async Task<IActionResult> Handle(SyncTestCaseCommand request, CancellationToken cancellationToken)
            {
                var existingTestCase = await _context.TestCases.FindAsync(request.Id);
                if (existingTestCase == null)
                {
                    var newTestCase = new TestCase
                    {
                        Id = request.Id,
                        InputTypeInt = request.InputTypeInt,
                        CodeQuestionId = request.CodeQuestionId,
                        ExpectedResultString = request.ExpectedResultString,
                        InputTypeArrayInt = request.InputTypeArrayInt,
                        InputTypeArrayString = request.InputTypeArrayString,
                        ExpectedResultInt = request.ExpectedResultInt,
                        ExpectedResultBoolean = request.ExpectedResultBoolean,
                        InputTypeBoolean = request.ExpectedResultBoolean,
                        InputTypeString = request.InputTypeString,

                    };
                    _context.TestCases.Add(newTestCase);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    existingTestCase.ExpectedResultBoolean = request.ExpectedResultBoolean;
                    existingTestCase.InputTypeBoolean = request.InputTypeBoolean;
                    existingTestCase.ExpectedResultInt = request.ExpectedResultInt;
                    existingTestCase.ExpectedResultString = request.ExpectedResultString;
                    existingTestCase.InputTypeArrayInt = request.InputTypeArrayInt;
                    existingTestCase.InputTypeArrayString = request.InputTypeArrayString;
                    existingTestCase.InputTypeInt = request.InputTypeInt;
                    existingTestCase.InputTypeString = request.InputTypeString;
                    existingTestCase.Id = request.Id;
                    existingTestCase.CodeQuestionId = request.CodeQuestionId;


                    await _context.SaveChangesAsync(cancellationToken);

                }




                return new OkObjectResult("done");
            }
        }
    }
}
