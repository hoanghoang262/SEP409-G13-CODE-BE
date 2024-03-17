using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using EventBus.Message.Event;
using MassTransit;
using MediatR;

namespace CourseService.API.MessageBroker
{
    public class EventExamAnswerHandler : IConsumer<AnswerExamEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventExamAnswerHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;

        }
        public async Task Consume(ConsumeContext<AnswerExamEvent> context)
        {
            var command = _mapper.Map<SyncExamAnswerCommand>(context.Message);
            var result = await _mediator.Send(command);
        }
    }
}
