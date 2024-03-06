using CourseService.API.Common.Mapping;
using EventBus.Message.Event;
using ForumService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ForumService.API.Fearture.Command
{
    public class SyncPostCommand : IRequest<IActionResult>, IMapFrom<PostEvent>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }


        public class SyncPostCommandHandler : IRequestHandler<SyncPostCommand, IActionResult>
        {
            private readonly ForumContext _context;
            public SyncPostCommandHandler(ForumContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(SyncPostCommand request, CancellationToken cancellationToken)
            {
                //var post = await _context.Posts.FindAsync(request.Id);

                //if (post == null)
                //{
                    var postForum = new Post
                    {
                     
                        Title = request.Title,  
                        Description = request.Description,  
                        PostContent = request.PostContent,  
                        CreatedBy=request.CreatedBy,
                        LastUpdate = request.LastUpdate,
                    };
                    _context.Posts.Add(postForum);
                    await _context.SaveChangesAsync(cancellationToken);
              //  }
                //else
                //{
                //    post.Title= request.Title;  
                //    post.Description= request.Description;
                //    post.LastUpdate= request.LastUpdate;
                //    post.PostContent= request.PostContent;
                //    await _context.SaveChangesAsync(cancellationToken);
                //}
                return new OkObjectResult("done");
            }
        }
    }
}
