using ForumService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetQuantityOfPostQuerry : IRequest<IActionResult>
    {

        public class GetQuantityOfPostQuerryHandler : IRequestHandler<GetQuantityOfPostQuerry, IActionResult>
        {
            private readonly ForumContext _dbContext;

            public GetQuantityOfPostQuerryHandler(ForumContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<IActionResult> Handle(GetQuantityOfPostQuerry request, CancellationToken cancellationToken)
            {
                var querry = await _dbContext.Posts.ToListAsync();
                if (querry == null)
                {
                    return new OkObjectResult(0);
                }
                var count = querry.Count();

                return new OkObjectResult(count);
            }
        }
    }
}
