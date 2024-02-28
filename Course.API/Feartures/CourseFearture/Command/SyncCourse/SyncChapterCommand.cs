using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;

using EventBus.Message.IntegrationEvent.PublishEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.PublishEvent;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class SyncChapterCommand : IRequest<IActionResult>, IMapFrom<ChapterEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }
        public class asyncChapterHandler : IRequestHandler<SyncChapterCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public asyncChapterHandler(CourseContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(SyncChapterCommand request, CancellationToken cancellationToken)
            {
                var chapter= await _context.Chapters.FindAsync(request.Id);
                if(chapter == null) {
                    var newChapter = new Chapter
                    {
                        Id=request.Id,
                        Name = request.Name,
                        CourseId = request.CourseId,
                        Part = request.Part,
                        IsNew = request.IsNew

                    };

                    _context.Chapters.Add(newChapter);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    chapter.Name = request.Name;
                    chapter.CourseId = request.CourseId;
                    chapter.Part = request.Part;
                    chapter.IsNew = request.IsNew;
                    await _context.SaveChangesAsync(cancellationToken);

                }
                


             
                return new OkObjectResult("done") ;
            }
        }
    }
   
}
