using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Fearture.Querries.TheoryQuestion;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TheoryQuestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TheoryQuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<TheoryQuestionDTO>> GetTheoryQuestionById(int id)
        {
            var query = new GetTheoryQuestionByIdQuery { TheoryQuestionId = id };
            var theoryQuestionDTO = await _mediator.Send(query);

            if (theoryQuestionDTO == null)
            {
                return NotFound();
            }

            return Ok(theoryQuestionDTO);
        }
    }
}
