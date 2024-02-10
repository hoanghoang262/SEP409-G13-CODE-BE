
using CourseService.API.Common.ModelDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class CreateCourseCommand : IRequest<IActionResult>
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public List<ChapterDTO> Chapters { get; set; }

        public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, IActionResult>
        {
            private readonly Content_ModerationContext _context;


            public CreateCourseHandler(Content_ModerationContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
            {

                var newCourse = new Course
                {
                    Name = request.Name,
                    Description = request.Description,
                    Picture = request.Picture,
                    Tag = request.Tag,
                    CreatedBy = request.UserId,


                };

                _context.Courses.Add(newCourse);
                await _context.SaveChangesAsync(cancellationToken);


                foreach (var chapterDto in request.Chapters)
                {
                    var newChapter = new Chapter
                    {
                        Name = chapterDto.Name,
                        Part = chapterDto.Part,
                        IsNew = chapterDto.IsNew,
                        CourseId = newCourse.Id

                    };

                    _context.Chapters.Add(newChapter);
                    await _context.SaveChangesAsync(cancellationToken);


                    foreach (var lessonDto in chapterDto.Lessons)
                    {
                        var newLesson = new Lesson
                        {
                            Title = lessonDto.Title,
                            VideoUrl = lessonDto.VideoUrl,
                            Description = lessonDto.Description,
                            Duration = lessonDto.Duration,
                            ChapterId = newChapter.Id
                            // Phần comments và questions sẽ được xử lý sau khi Lesson được thêm vào cơ sở dữ liệu
                        };

                        _context.Lessons.Add(newLesson);
                        await _context.SaveChangesAsync(cancellationToken);

                        // Thêm các comment cho bài học mới


                        // Thêm các câu hỏi cho bài học mới
                        foreach (var questionDto in lessonDto.Questions)
                        {
                            var newQuestion = new Question
                            {
                                ContentQuestion = questionDto.ContentQuestion,
                                AnswerA = questionDto.AnswerA,
                                AnswerB = questionDto.AnswerB,
                                AnswerC = questionDto.AnswerC,
                                AnswerD = questionDto.AnswerD,
                                CorrectAnswer = questionDto.CorrectAnswer,
                                Time = questionDto.Time,
                                VideoId = newLesson.Id
                            };

                            _context.Questions.Add(newQuestion);
                            await _context.SaveChangesAsync(cancellationToken);
                        }
                    }
                }


                return new OkObjectResult("Your course will be sent to Admin for moderation");
            }
        }
    }
   
}
