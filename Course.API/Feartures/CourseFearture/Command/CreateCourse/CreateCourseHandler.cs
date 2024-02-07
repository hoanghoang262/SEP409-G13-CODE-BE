using CourseService.API.Common.ModelDTO;
using EventBus.Message.IntegrationEvent.Event;
using MediatR;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class CreateCourseHandler : IRequestHandler<MessageCommand, CourseMessage>
    {
        private readonly CourseContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public CreateCourseHandler(CourseContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService= cloudinaryService;
        }
        public async Task<CourseMessage> Handle(MessageCommand request, CancellationToken cancellationToken)
        {
          
            var course = new Course
            {
                Name=request.Name,
                Description=request.Description,
                Picture=request.Picture,
                Tag=request.Tag,
                UserId=request.UserId, 
                
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync(cancellationToken);

            //foreach (var chapterDto in request.Chapters)
            //{
            //    var chapter = new Chapter
            //    {
            //        Name=chapterDto.Name,
            //        CourseId=course.Id,
            //        Part=chapterDto.Part,
            //        IsNew=false
                    
            //    };
            //    _context.Chapters.Add(chapter);
            //    await _context.SaveChangesAsync(cancellationToken);

            //    foreach (var lessonDto in chapterDto.Lessons)
            //    {
            //        var VideoUrl= await _cloudinaryService.UploadVideoAsync(lessonDto.VideoByte);
            //        var lesson = new Lesson
            //        {
            //            Title=lessonDto.Title,
            //            VideoUrl=VideoUrl,
            //            ChapterId= chapter.Id,
            //            Description=lessonDto.Description,
            //            Duration=lessonDto.Duration
            //        };
            //        _context.Lessons.Add(lesson);
            //        await _context.SaveChangesAsync(cancellationToken);
            //    }
            //}
            return null;
        }
    }
}
