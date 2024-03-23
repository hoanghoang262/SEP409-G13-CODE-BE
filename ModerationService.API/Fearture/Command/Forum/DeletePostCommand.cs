using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;
using System.Security.Cryptography.Pkcs;

namespace ModerationService.API.Fearture.Command.Forum
{
    public class DeletePostCommand : IRequest<int>
    {
       public int postId { get; set; }

        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, int>
        {
            private readonly Content_ModerationContext _context;

            public DeletePostCommandHandler(Content_ModerationContext context)
            {
                _context = context;

            }
            public async Task<int> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id.Equals(request.postId));

                _context.Posts.Remove(post);
               await _context.SaveChangesAsync();

                return post.Id;
            }
        }
    }
}
