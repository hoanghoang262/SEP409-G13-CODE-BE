

using CourseService.API.Common.Mapping;
using EventBus.Message.IntegrationEvent.Event;
using MediatR;

namespace CourseService.API.Feartures.CourseFearture.Queries
{
    public class MessageCommand :IRequest<LoginEvent>,IMapFrom<LoginEvent>
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
       
    }
}
