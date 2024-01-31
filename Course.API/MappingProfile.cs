
using AutoMapper;

using CourseService.API.Feartures.CourseFearture.Queries;
using EventBus.Message.IntegrationEvent.Event;


namespace Course.API
{
    public class MappingProfile :Profile
    {
        public MappingProfile() {
            CreateMap<MessageCommand, UserIdMessage>().ReverseMap();
          
        }
    }
}
