


using AutoMapper;
using CourseService.API.Common.Mapping;
using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;

using MediatR;
using System.Globalization;

namespace CourseService.API.Feartures.CourseFearture.Queries
{

    //public class GetMessageHandler : IRequestHandler<MessageCommand, CourseMessage>
    //{
    //    private readonly IMediator mediator;
    //    private readonly IMapper _mapper;
    //    public GetMessageHandler(IMediator _mediator, IMapper mapper)
    //    {

    //        mediator = _mediator;
    //        _mapper = mapper;

    //    }
    //    public async Task<CourseMessage> Handle(MessageCommand request, CancellationToken cancellation)
    //    {
    //        if (request == null)
    //        {
    //            return null;
    //        }
    //        CourseMessage idMessage = null;




    //        return idMessage;

    //    }

    //}
}


