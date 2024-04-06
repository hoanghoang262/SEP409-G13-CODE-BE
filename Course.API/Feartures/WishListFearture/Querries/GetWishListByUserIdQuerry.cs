using Contract.SeedWork;
using Contract.Service.Message;
using CourseService.API.Common.DTO;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CourseService.API.Feartures.WishListFearture.Querries
{
    public class GetWishListByUserIdQuerry : IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? CourseName { get; set; }
        public string? Tag { get; set; }
        public class GetWishListByUserIdQuerryHandler : IRequestHandler<GetWishListByUserIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly GetUserInfoService _service;

            public GetWishListByUserIdQuerryHandler(CourseContext context, GetUserInfoService service)
            {
                _context = context;
                _service = service;
            }

            public async Task<IActionResult> Handle(GetWishListByUserIdQuerry request, CancellationToken cancellationToken)
            {
                var querry = (from w in _context.Wishlists
                              join c in _context.Courses on w.CourseId equals c.Id
                              where( (w.UserId == request.UserId)&&
                              (string.IsNullOrEmpty(request.CourseName)||c.Name.Contains(request.CourseName))&&
                                    (string.IsNullOrEmpty(request.Tag) || c.Tag.Contains(request.Tag)))
                               
                              select new
                              {
                                  CourseId = w.CourseId,
                                  Name = c.Name,
                                  Id = w.Id,
                                  Description = c.Description,
                                  Picture = c.Description,
                                  CreatedAt = c.CreatedAt,
                                  Tag = c.Tag,
                                  UserId = c.CreatedBy
                              }).ToList();
                var totalItems =  querry.Count();

                var resultList = querry
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                List<WishListDTO> wishList = new List<WishListDTO>();
                foreach (var c in resultList)
                {
                    var userInfo = await _service.SendUserId(c.UserId);
                    if (userInfo.Id == 0)
                    {
                        return new BadRequestObjectResult(Message.MSG24);
                    }
                   

                    var dto = new WishListDTO()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UserId = c.UserId,
                        CreatedAt = c.CreatedAt,
                        Tag = c.Tag,
                        Picture = userInfo.Picture,
                        UserName = userInfo.Name,
                        Description = c.Description,
                        CourseId = c.CourseId
                    };
                    wishList.Add(dto);
                }
                var result = new PageList<WishListDTO>(wishList, totalItems, request.Page, request.PageSize);

                return new OkObjectResult(result);
            }
        }
    }
}
