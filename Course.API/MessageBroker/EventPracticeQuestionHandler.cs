using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using EventBus.Message.Event;
using MassTransit;
using MassTransit.Testing;
using MediatR;

namespace CourseService.API.MessageBroker
{
    public class EventPracticeQuestionHandler : IConsumer<PracticeQuestionEvent>
    {
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EventHandler> logger;
        public EventPracticeQuestionHandler(ILogger<EventHandler> _logger, IMediator _mediator, IMapper mapper)
        {

            mediator = _mediator;
            _mapper = mapper;
            logger = _logger;
        }
        public async Task Consume(ConsumeContext<PracticeQuestionEvent> context)
        {
            var command = _mapper.Map<SyncPracticeQuestionCommand>(context.Message);
            var result = await mediator.Send(command);
        }
    }
}
