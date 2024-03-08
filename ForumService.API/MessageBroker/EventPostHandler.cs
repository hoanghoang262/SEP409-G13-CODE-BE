using AutoMapper;
using EventBus.Message.Event;
using ForumService.API.Fearture.Command;
using MassTransit;

using MediatR;

namespace ForumService.API.MessageBroker
{
    public class EventPostHandler : IConsumer<PostEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public EventPostHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<PostEvent> context)
        {
            var command = _mapper.Map<SyncPostCommand>(context.Message);
            var result = await _mediator.Send(command);
        }
    }
}
