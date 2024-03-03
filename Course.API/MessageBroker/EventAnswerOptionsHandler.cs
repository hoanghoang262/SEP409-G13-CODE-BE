using AutoMapper;

using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using EventBus.Message.Event;
using MassTransit;

using MediatR;

namespace CourseService.API.MessageBroker
{
    public class EventAnswerOptionsHandler : IConsumer<AnswerOptionsEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventAnswerOptionsHandler(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;

        }
        public async Task Consume(ConsumeContext<AnswerOptionsEvent> context)
        {
            var command = _mapper.Map<SyncAnswerOptionCommand>(context.Message);
            var result = await _mediator.Send(command);
        }
    }
}
