using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using EventBus.Message.Event;
using MassTransit;
using MediatR;

namespace CourseService.API.MessageBroker
{
    public class EventExamHandler : IConsumer<QuestionExamEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventExamHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;

        }
        public async Task Consume(ConsumeContext<QuestionExamEvent> context)
        {
            var command = _mapper.Map<SyncExamCommand>(context.Message);
            var result = await _mediator.Send(command);
        }
    }
}
