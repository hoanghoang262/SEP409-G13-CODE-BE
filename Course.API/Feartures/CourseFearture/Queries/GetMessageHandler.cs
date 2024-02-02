


using AutoMapper;
using CourseService.API.Common.Mapping;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;

using MediatR;
using System.Globalization;

namespace CourseService.API.Feartures.CourseFearture.Queries
{
    public class MessageCommand : IRequest<UserIdMessage>, IMapFrom<UserIdMessage>
    {
        public int UserId { get; set; }


        public class GetMessageHandler : IRequestHandler<MessageCommand, UserIdMessage>, IConsumer<UserIdMessage>
        {
            private readonly IMediator mediator;
            private readonly IMapper _mapper;

            public GetMessageHandler(IMediator _mediator, IMapper mapper)
            {

                mediator = _mediator;
                _mapper = mapper;

            }
            public async Task Consume(ConsumeContext<UserIdMessage> context)
            {
                var command = _mapper.Map<MessageCommand>(context.Message);
                var result = await mediator.Send(command);
            }

            public async Task<UserIdMessage> Handle(MessageCommand request, CancellationToken cancellation)
            {
                if (request == null)
                {
                    return null;
                }
                UserIdMessage idMessage = null;




                return idMessage;

            }



        }
    }
}
