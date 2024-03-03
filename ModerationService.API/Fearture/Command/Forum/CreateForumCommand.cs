using MediatR;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreateForumCommand : IRequest<int>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }

        public class CreateForumCommandHandler : IRequestHandler<CreateForumCommand, int>
        {
            private readonly Content_ModerationContext _context;

            public CreateForumCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateForumCommand request, CancellationToken cancellationToken)
            {
                var forum = new Forum
                {
                    Title = request.Title,
                    Description = request.Description,
                    CreatedBy = request.CreatedBy,
                    LastUpdate = request.LastUpdate,
                };
                _context.Forums.Add(forum);
                await _context.SaveChangesAsync();

                return forum.Id;
            }
        }

    }
}
