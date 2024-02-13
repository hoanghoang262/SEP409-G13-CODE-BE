using Contract.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ModerationService.API.Fearture.Querries.Moderations
{
    public class GetModerationQuerry : IRequest<PageList<Moderation>>
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 5;

        public class GetModerationQuerryHandler : IRequestHandler<GetModerationQuerry, PageList<Moderation>>
        {
            private readonly Content_ModerationContext _context;

            public GetModerationQuerryHandler(Content_ModerationContext context)
            {
                _context = context;
            }
            public async Task<PageList<Moderation>> Handle(GetModerationQuerry request, CancellationToken cancellationToken)
            {

                var querry = await _context.Moderations.ToListAsync();
                if(querry == null) {
                    return null;
                   
                }
                var totalItems = querry.Count();
                var item= querry.Skip((request.page - 1) * request.pageSize).Take(request.pageSize).ToList();
                var result = new PageList<Moderation>(item, totalItems, request.page, request.pageSize);
                return result;

            }
        }
    }
}
