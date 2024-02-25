
using CourseService.API.Common.ModelDTO;
using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class CreateCourseCommand : IRequest<IActionResult>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public List<ChapterDTO> Chapters { get; set; }

        public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, IActionResult>
        {
            private readonly Content_ModerationContext _context;

            private readonly UserIdCourseGrpcService service;


            public CreateCourseHandler(Content_ModerationContext context, UserIdCourseGrpcService _service)
            {
                _context = context;
                service = _service;
            }
            public async Task<IActionResult> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
            {
                var user = await service.SendUserId(request.CreatedBy);

                var newCourse = new Course
                {
                    Name = request.Name,
                    Description = request.Description,
                    Picture = request.Picture,
                    Tag = request.Tag,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = request.CreatedAt,


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

                    foreach (var codequestionDto in chapterDto.CodeQuestions)
                    {
                        var newCodeQuestion = new CodeQuestion
                        {
                            ChapterId = newChapter.Id,
                            Description = codequestionDto.Description,
                        };
                        _context.CodeQuestions.Add(newCodeQuestion);
                        await _context.SaveChangesAsync(cancellationToken);

                        foreach (var testcaseDto in codequestionDto.TestCases)
                        {
                            var newTestCase = new TestCase
                            {
                                InputTypeInt = testcaseDto.InputTypeInt,
                                CodeQuestionId = newCodeQuestion.Id,
                                ExpectedResultString = testcaseDto.ExpectedResultString,
                                InputTypeArrayInt = testcaseDto.InputTypeArrayInt,
                                InputTypeArrayString = testcaseDto.InputTypeArrayString,
                                ExpectedResultInt = testcaseDto.ExpectedResultInt,
                                ExpectedResultBoolean = testcaseDto.ExpectedResultBoolean,
                                InputTypeBoolean = testcaseDto.ExpectedResultBoolean,
                                InputTypeString = testcaseDto.InputTypeString
                            };
                            _context.TestCases.Add(newTestCase);
                            await _context.SaveChangesAsync(cancellationToken);

                        }

                    }


                foreach (var lessonDto in chapterDto.Lessons)
                {
                    var newLesson = new Lesson
                    {
                        Title = lessonDto.Title,
                        VideoUrl = lessonDto.VideoUrl,
                        Description = lessonDto.Description,
                        Duration = lessonDto.Duration,
                        ChapterId = newChapter.Id,
                        IsCompleted = false

                    };

                    _context.Lessons.Add(newLesson);
                    await _context.SaveChangesAsync(cancellationToken);


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
            var moderation = new Moderation
            {
                CourseId = newCourse.Id,
                ChangeType = "Add",
                CreatedBy = user.Name,
                ApprovedContent = "Add a new course",
                Status = "Pending",
                CreatedAt = newCourse.CreatedAt


            };
            await _context.Moderations.AddAsync(moderation);
            await _context.SaveChangesAsync(cancellationToken); 




                return new OkObjectResult("Your course will be sent to Admin for moderation");
        }
    }
}
   
}
