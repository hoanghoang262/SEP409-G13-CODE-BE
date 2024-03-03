
using AutoMapper;
using Contract.SeedWork;
using CourseService.API.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;


namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetAllCourseQuerry : IRequest<PageList<Course>>
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public class GetAllCoursesHandler : IRequestHandler<GetAllCourseQuerry, PageList<Course>>
        {
            private readonly IMediator mediator;
            private readonly IMapper _mapper;
            private readonly Course_DeployContext _context;
            public GetAllCoursesHandler(IMediator _mediator, IMapper mapper, Course_DeployContext context)
            {

                mediator = _mediator;
                _mapper = mapper;
                _context = context;

            }
            public async Task<PageList<Course>> Handle(GetAllCourseQuerry request, CancellationToken cancellation)
            {
                var query = await _context.Courses.ToListAsync();
                if (query == null)
                {
                    return null;
                }
                var totalItems =
                    query.Count();
                var courseList = query.Skip((request.page - 1) * request.pageSize).Take(request.pageSize).ToList();

                var result = new PageList<Course>(courseList, totalItems, request.page, request.pageSize);
                return result;
            }

        }

    }


}


