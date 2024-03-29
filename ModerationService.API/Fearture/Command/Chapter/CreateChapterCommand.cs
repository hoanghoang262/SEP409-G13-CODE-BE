using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command
{
    public class CreateChapterCommand : IRequest<IActionResult>
    {
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }
    }

    public class AddChapterCommandHandler : IRequestHandler<CreateChapterCommand, IActionResult>
    {
        private readonly Content_ModerationContext _context;

        public AddChapterCommandHandler(Content_ModerationContext moderationContext)
        {
            _context = moderationContext;
        }

        public async Task<IActionResult> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            // validate input

            if (string.IsNullOrEmpty(request.Name) || request.CourseId == 0 || request.Part == 0)
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

            var chapter = new Chapter
            {
                Name = request.Name,
                CourseId = request.CourseId,
                Part = request.Part,
                IsNew = false
            };

            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();

            return new OkObjectResult(chapter);
        }

    }
}
