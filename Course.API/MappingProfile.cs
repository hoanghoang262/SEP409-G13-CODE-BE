
using AutoMapper;

using CourseService.API.Feartures.CourseFearture.Queries;
using EventBus.Message.IntegrationEvent.Event;


namespace CourseService.API
{
    public class MappingProfile :Profile
    {
        public MappingProfile() {
            CreateMap<MessageCommand, LoginEvent>().ReverseMap();
          
        }
    }
}
