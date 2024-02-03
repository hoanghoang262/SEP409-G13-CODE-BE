
using AutoMapper;
using CourseService;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Feartures.CourseFearture.Queries;
using EventBus.Message.IntegrationEvent.Event;


namespace CourseService.API
{
    public class MappingProfile :Profile
    {
        public MappingProfile() {
            CreateMap<MessageCommand, UserIdMessage>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
          
        }
    }
}
