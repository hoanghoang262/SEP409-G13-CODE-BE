using Contract.SeedWork;
using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.DTO;
using ModerationService.API.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ModerationService.API.Feature.Queries
{
    public class GetModerationCourseQuerry : IRequest<PageList<ModerationDTO>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? CourseName { get; set; }

        public class GetModerationQuerryHandler : IRequestHandler<GetModerationCourseQuerry, PageList<ModerationDTO>>
        {
            private readonly Content_ModerationContext _context;
            private readonly UserIdCourseGrpcService _service;

            public GetModerationQuerryHandler(Content_ModerationContext context, UserIdCourseGrpcService service)
            {
                _context = context;
                _service = service;
            }

            public async Task<PageList<ModerationDTO>> Handle(GetModerationCourseQuerry request, CancellationToken cancellationToken)
            {
           
                List<Moderation> moderations;
                if (string.IsNullOrEmpty(request.CourseName))
                {
                    moderations = await _context.Moderations.Include(c=>c.Course).Where(x=>x.Course.IsCompleted == true).ToListAsync();
                }
                else
                {
                    moderations = await _context.Moderations.Include(c => c.Course).Where(x => x.CourseName.Contains(request.CourseName) && x.Course.IsCompleted == true).ToListAsync();
                }

                if (moderations == null)
                {
                    return null;
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
                        CoursePicture=moderation.Course.Picture,
                        CourseDescription=moderation.Course.Description,

                       
                    };
                    moderationDTOs.Add(moderationDTO);
                }

                return new PageList<ModerationDTO>(moderationDTOs, totalItems, request.Page, request.PageSize);
            }
        }
    }
}
