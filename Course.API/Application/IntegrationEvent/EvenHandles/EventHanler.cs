


using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Queries;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;
using MassTransit.Testing;
using MediatR;

namespace CourseService.API.IntegrationEvent.EvenHandles
{
    public class EventHanler : IConsumer<LoginEvent>
    {
       
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        public EventHanler( IMediator _mediator, IMapper mapper)
        {
           
            mediator = _mediator;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<LoginEvent> context)
        {
            var command = _mapper.Map<MessageCommand>(context.Message);
            var result = await mediator.Send(command);
        }
    }
}
