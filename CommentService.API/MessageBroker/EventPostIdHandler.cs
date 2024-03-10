
using AutoMapper;
using CommentService.API.Fearture.Command;
using EventBus.Message.Event;
using MassTransit;

using MediatR;

namespace ForumService.API.MessageBroker
{
    public class EventPostIdHandler : IConsumer<PostIdEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public EventPostIdHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<PostIdEvent> context)
        {
            var command = _mapper.Map<DeleteAllCommentCommand>(context.Message);
            var result = await _mediator.Send(command);
        }
    }
}
