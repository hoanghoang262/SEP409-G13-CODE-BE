using AutoMapper;

namespace CourseService.API.Common.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) =>
            profile.CreateMap(typeof(T), GetType());

    }
}
