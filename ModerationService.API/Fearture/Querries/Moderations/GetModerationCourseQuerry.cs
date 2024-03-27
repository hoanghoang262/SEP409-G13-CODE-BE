using Contract.SeedWork;
using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.Models;

namespace ModerationService.API.Feature.Queries
{
    public class GetModerationCourseQuerry : IRequest<IActionResult>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? CourseName { get; set; }
        public string? Tag { get; set; }

        public class GetModerationQuerryHandler : IRequestHandler<GetModerationCourseQuerry, IActionResult>

        {
            private readonly Content_ModerationContext _context;
            private readonly UserIdCourseGrpcService _service;

            public GetModerationQuerryHandler(Content_ModerationContext context, UserIdCourseGrpcService service)
            {
                _context = context;
                _service = service;
            }
            public async Task<IActionResult> Handle(GetModerationCourseQuerry request, CancellationToken cancellationToken)
            {

                List<Moderation> moderations = new List<Moderation>();
                if (string.IsNullOrEmpty(request.CourseName) && string.IsNullOrEmpty(request.Tag))
                {
                    moderations = await _context.Moderations
                        .Include(c => c.Course)
                        .Where(x => x.CourseId!=null).ToListAsync();
                }
                if (!string.IsNullOrEmpty(request.CourseName) && string.IsNullOrEmpty(request.Tag))
                {
                    moderations = await _context.Moderations
                        .Include(c => c.Course)
                        .Where(x => x.CourseName
                        .Contains(request.CourseName) && x.CourseId != null).ToListAsync();
                }
                if (string.IsNullOrEmpty(request.CourseName) && !string.IsNullOrEmpty(request.Tag))
                {
                    moderations = await _context.Moderations
                        .Include(c => c.Course)
                        .Where(x => x.Status
                        .Equals(request.Tag) && x.CourseId != null).ToListAsync();
                }
                if (!string.IsNullOrEmpty(request.CourseName) && !string.IsNullOrEmpty(request.Tag))
                {

                    moderations = await _context.Moderations
                        .Include(c => c.Course)
                        .Where(x => x.Status
                        .Contains(request.Tag) && x.CourseName.Contains(request.CourseName) && x.CourseId!=null).ToListAsync();

                }

                var totalItems = moderations.Count;
                var items = moderations
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                // Now for each ModerationDTO, retrieve UserName from UserIdCourseGrpcService
                var moderationDTOs = new List<ModerationDTO>();
                foreach (var moderation in items)
                {
                    // Call the microservice to get the UserName for the given CreatedBy
                    var userName = await _service.SendUserId((int)moderation.CreatedBy);
                    var moderationDTO = new ModerationDTO
                    {
                        Id = moderation.Id,
                        CourseId = moderation.CourseId,
                        ChangeType = moderation.ChangeType,
                        ApprovedContent = moderation.ApprovedContent,
                        CreatedBy = moderation.CreatedBy,
                        CreatedAt = moderation.CreatedAt,
                        Status = moderation.Status,
                        CourseName = moderation.CourseName,
                        UserName = userName.Name,
                        CoursePicture = moderation.Course.Picture,
                        CourseDescription = moderation.Course.Description,
                        Tag = moderation.Course.Tag,


                    };
                    moderationDTOs.Add(moderationDTO);
                }

                var result = new PageList<ModerationDTO>(moderationDTOs, totalItems, request.Page, request.PageSize);

                return new OkObjectResult(result);
            }
        }
    }
}
