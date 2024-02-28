using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using EventBus.Message.IntegrationEvent.PublishEvent;
using MassTransit;
using MediatR;
using ModerationService.API.Common.PublishEvent;

namespace CourseService.API.Application.MessageBroker.EvenHandles
{
    public class EventChapterHandler : IConsumer<ChapterEvent>
    {

        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<EventHandler> logger;
        public EventChapterHandler(ILogger<EventHandler> _logger, IMediator _mediator, IMapper mapper)
        {

            mediator = _mediator;
            _mapper = mapper;
            logger = _logger;
        }

        public async Task Consume(ConsumeContext<ChapterEvent> context)
        {
            var command = _mapper.Map<SyncChapterCommand>(context.Message);
            var result = await mediator.Send(command);

        }
    }
}
