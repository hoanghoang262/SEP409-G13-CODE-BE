

using AutoMapper;
using Contract.SeedWork;
using Contract.Service.Message;
using CourseService.API.Common.ModelDTO;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetAllCourseQuerry : IRequest<IActionResult>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? CourseName { get; set; }
        public string? Tag { get; set; }
        public int? UserId { get; set; }
        public class GetAllCoursesHandler : IRequestHandler<GetAllCourseQuerry, IActionResult>
        {
            private readonly IMediator mediator;
            private readonly IMapper _mapper;
            private readonly CourseContext _context;
            private readonly GetUserInfoService _service;
            public GetAllCoursesHandler(IMediator _mediator, IMapper mapper, CourseContext context, GetUserInfoService service)
            {
                mediator = _mediator;
                _mapper = mapper;
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(GetAllCourseQuerry request, CancellationToken cancellation)
            {
                IQueryable<Course> query = _context.Courses;
                if (!string.IsNullOrEmpty(request.CourseName))
                {
                    query = query.Where(c => c.Name.Contains(request.CourseName));
                }
                if (!string.IsNullOrEmpty(request.Tag))
                {
                    query = query.Where(c => c.Tag.Equals(request.Tag));
                }
                var totalItems = await query.CountAsync();
                if (totalItems <= 0)
                {
                    return new NotFoundObjectResult(totalItems);
                }
                var courseList = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                List<CourseDTO> courseDTOList = new List<CourseDTO>();
                foreach (var item in courseList)
                {
                    var userInfo = await _service.SendUserId(item.CreatedBy);
                    bool isUserEnrolled = _context.Enrollments.Any(e => e.UserId == request.UserId && e.CourseId == item.Id);
                    bool isInWishList= _context.Wishlists.Any(e=>e.UserId==request.UserId && e.CourseId==item.Id);
                    var dto = new CourseDTO
                    {
                        CreatedAt = item.CreatedAt,
                        Description = item.Description,
                        Id = item.Id,
                        Name = item.Name,
                        Picture = item.Picture,
                        Tag = item.Tag,
                        UserId = item.CreatedBy,
                        UserName = userInfo.Name,
                        Enrolled = isUserEnrolled ? "Continue Studying" : "Enroll",
                        IsInWishList=isInWishList,
                        Price=item.Price,
                    };
                    courseDTOList.Add(dto);
                }
                var result = new PageList<CourseDTO>(courseDTOList, totalItems, request.Page, request.PageSize);
                return new OkObjectResult(result);
            }
        }
    }
}
