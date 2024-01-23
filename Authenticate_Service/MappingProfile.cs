using Account.API.Model;
using AutoMapper;
using EventBus.Message.IntegrationEvent.Event;
using EventBus.Message.IntegrationEvent.Interfaces;

namespace Authenticate_Service
{
    public class MappingProfile :Profile
    {
        public MappingProfile() {
            CreateMap<LoginModels, LoginEvent>().ReverseMap();
          
        }
    }
}
