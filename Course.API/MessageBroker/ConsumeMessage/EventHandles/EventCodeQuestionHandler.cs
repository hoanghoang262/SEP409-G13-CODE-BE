using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;
using MassTransit.Testing;
using MediatR;

namespace CourseService.API.MessageBroker.ConsumeMessage.EventHandles
{
    public class EventCodeQuestionHandler : IConsumer<CodeQuestionEvent>
    {
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EventHandler> logger;
        public EventCodeQuestionHandler(ILogger<EventHandler> _logger, IMediator _mediator, IMapper mapper)
        {

            mediator = _mediator;
            _mapper = mapper;
            logger = _logger;
        }
        public async Task Consume(ConsumeContext<CodeQuestionEvent> context)
        {
            var command = _mapper.Map<SyncCodeQuestionCommand>(context.Message);
            var result = await mediator.Send(command);
        }
    }
}
