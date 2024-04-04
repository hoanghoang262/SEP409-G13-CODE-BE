using AutoMapper;
using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using CourseService.API.Feartures.EnrollmentFeature.Command;
using EventBus.Message.Event;
using MassTransit;
using MediatR;

namespace CourseService.API.MessageBroker
{
    public class EventUserEnrollHandler : IConsumer<UserEnrollEvent>
    {
        private readonly IMediator mediator;
        private readonly IMapper _mapper;
      
        public EventUserEnrollHandler( IMediator _mediator, IMapper mapper)
        {

            mediator = _mediator;
            _mapper = mapper;
          
        }
        public async Task Consume(ConsumeContext<UserEnrollEvent> context)
        {
            var command = _mapper.Map<SyncEnrollCourseAfterPaymentCommand>(context.Message);
            var result = await mediator.Send(command);
        }
    }
}
