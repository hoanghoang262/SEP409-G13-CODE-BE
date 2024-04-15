using AutoMapper;
using EventBus.Message.Event;
using MassTransit;
using MediatR;
using NotificationService.API.Fearture.NotificationFearture.Command;

namespace NotificationService.API.MessageBroker
{
    public class EventNotificationPostHandler : IConsumer<NotificationPostEvent>
    {
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EventHandler> logger;

        public EventNotificationPostHandler(ILogger<EventHandler> _logger, IMediator _mediator, IMapper mapper)
        {

            mediator = _mediator;
            _mapper = mapper;
            logger = _logger;
        }

        public async Task Consume(ConsumeContext<NotificationPostEvent> context)
        {
            var command = _mapper.Map<CreateNotificationPostCommand>(context.Message);
            var result = await mediator.Send(command);
        }
    }
}
