using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Forum
{
    public class UpdatePostCommand : IRequest<IActionResult>
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }

        public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, IActionResult>
        {
            private readonly Content_ModerationContext _context;
            public UpdatePostCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                // Validate request
                if (string.IsNullOrEmpty(request.Title)
                    || string.IsNullOrEmpty(request.Description)
                    || string.IsNullOrEmpty(request.PostContent))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }
                if (request.Title.Length > 256)
                {
                    return new BadRequestObjectResult(Message.MSG27);
                }

                var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));
                if (post == null)
                {
                    return new BadRequestObjectResult(Message.MSG34);
                }

                post.Title = request.Title;
                post.PostContent = request.PostContent;
                post.Description = request.Description;
                post.LastUpdate = DateTime.Now;

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                return new OkObjectResult(post);

            }
        }
    }
}
