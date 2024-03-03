using CourseService.API.Feartures.CourseFearture.Queries.CourseQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckAnswerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CheckAnswerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CheckAnswer(int questionId,  List<int> selectedOptionIds)
        {
            var command = new CheckAnswerQuestion { QuestionId = questionId, SelectedOptionIds = selectedOptionIds };
            var isCorrect = await _mediator.Send(command);

            return isCorrect;
        }
    }
}
