


using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;
using MassTransit.Testing;
using MediatR;

namespace CourseService.API.IntegrationEvent.EvenHandles
{
    public class EventHanler : IConsumer<CourseMessage>
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

        public async Task Consume(ConsumeContext<CourseMessage> context)
        {
            var command = _mapper.Map<MessageCommand>(context.Message);
            var result =  await mediator.Send(command);
            
        }
    }
}
