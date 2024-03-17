using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class UpdateChapterCommand : IRequest<ActionResult<Chapter>>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }
    }

    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, ActionResult<Chapter>>
    {
        private readonly Content_ModerationContext _context;

        public UpdateChapterCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<ActionResult<Chapter>> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            // validate input
            if (request.Name == null || request.Part == null || request.IsNew == null)
            {
                return new BadRequestObjectResult(Message.MSG11);
            }

            // invalid part number
            if (request.Part < 0)
            {
                return new BadRequestObjectResult(Message.MSG26);
            }

            // string length
            if (request.Name.Length > 256)
            {
                return new BadRequestObjectResult(Message.MSG27);
            }

            // check if chapter exists
            var chapter = await _context.Chapters.FindAsync(request.Id);
            if (chapter == null)
            {
                return new BadRequestObjectResult(Message.MSG28);
            }

            chapter.Name = request.Name;
            chapter.Part = request.Part;
            chapter.IsNew = request.IsNew;

            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();

            return chapter;
        }
    }
}
