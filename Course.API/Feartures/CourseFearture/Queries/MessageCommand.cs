

using CourseService.API.Common.Mapping;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;
using MediatR;

namespace CourseService.API.Feartures.CourseFearture.Queries
{
    public class MessageCommand : IRequest<UserIdMessage>,IMapFrom<UserIdMessage>
    {
       public int UserId { get; set; }   

    }
}
