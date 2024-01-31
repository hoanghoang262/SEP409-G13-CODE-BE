


using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Queries;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;
using MassTransit.Testing;
using MediatR;

namespace CourseService.API.IntegrationEvent.EvenHandles
{
    public class EventHanler : IConsumer<UserIdMessage>
    {
       
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EventHandler> logger;
        public EventHanler(ILogger<EventHandler>_logger, IMediator _mediator, IMapper mapper)
        {
           
            mediator = _mediator;
            _mapper = mapper;
            logger = _logger;
        }

        public async Task Consume(ConsumeContext<UserIdMessage> context)
        {
            var command = _mapper.Map<MessageCommand>(context.Message);
            var result =  await mediator.Send(command);
            
        }
    }
}
