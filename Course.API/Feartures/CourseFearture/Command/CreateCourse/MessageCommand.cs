using AutoMapper;
using CourseService.API.Common.Mapping;
using EventBus.Message.IntegrationEvent.Event;
using MediatR;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class MessageCommand : IRequest<CourseMessage>, IMapFrom<CourseMessage>
    {
       
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? UserId { get; set; }
     


    }
}
