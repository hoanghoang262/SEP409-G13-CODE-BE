using Contract.SeedWork;
using GrpcServices;
using MassTransit.Initializers;
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
    public class GetModerationPostQuerry : IRequest<PageList<ModerationDTO>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? PostTitle { get; set; }

        public class GetModerationPostQuerryHandler : IRequestHandler<GetModerationPostQuerry, PageList<ModerationDTO>>
        {
            private readonly Content_ModerationContext _context;
            private readonly UserIdCourseGrpcService _service;

            public GetModerationPostQuerryHandler(Content_ModerationContext context, UserIdCourseGrpcService service)
            {
                _context = context;
                _service = service;
            }

            public async Task<PageList<ModerationDTO>> Handle(GetModerationPostQuerry request, CancellationToken cancellationToken)
            {
                List<Moderation> moderations;
                if (string.IsNullOrEmpty(request.PostTitle))
                {
                    moderations = await _context.Moderations.Where(x => x.PostId != null).ToListAsync();
                }
                else
                {
                    moderations = await _context.Moderations.Where(x => x.PostTitle.Contains(request.PostTitle) && x.PostId != null).ToListAsync();
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
                    var userName = await _service.SendUserId(moderation.CreatedBy);
                    var postDes = await _context.Posts.FirstOrDefaultAsync(x => x.Id.Equals(moderation.PostId));
                    var moderationDTO = new ModerationDTO
                    {
                        Id = moderation.Id,
                        PostId = moderation.PostId,
                        ChangeType = moderation.ChangeType,
                        ApprovedContent = moderation.ApprovedContent,
                        CreatedBy = moderation.CreatedBy,
                        CreatedAt = moderation.CreatedAt,
                        Status = moderation.Status,
                        PostTitle = moderation.PostTitle,
                        UserName = userName.Name,
                        PostDescription = postDes.Description,
                        PostContent= postDes.PostContent,
                        PostCreateAt=postDes.LastUpdate,

                    };
                    moderationDTOs.Add(moderationDTO);
                }

                return new PageList<ModerationDTO>(moderationDTOs, totalItems, request.Page, request.PageSize);
            }
        }
    }
}
