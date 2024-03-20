using Contract.Service.Message;
using GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class UpdateCourseCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

       // public List<ChapterDTO> Chapters { get; set; }

        public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, IActionResult>
        {
            private readonly Content_ModerationContext _context;
            private readonly GetCourseIdGrpcServices services;
            private readonly UserIdCourseGrpcService service;

            public UpdateCourseCommandHandler(Content_ModerationContext context, GetCourseIdGrpcServices _services, UserIdCourseGrpcService _service)
            {
                _context = context;
                services = _services;
                service=_service;
            }

            public async Task<IActionResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
            {
                // validate input
                if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Description) || string.IsNullOrEmpty(request.Picture) || string.IsNullOrEmpty(request.Tag))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                // string length
                if (request.Name.Length > 256)
                {
                    return new BadRequestObjectResult(Message.MSG27);
                }

                // check if the course exist
                var courseId = await services.SendCourseId(request.Id);
                if (courseId == null)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }
                
                // Check if the user exist
                var user = await service.SendUserId(request.CreatedBy);
                if (user == null)
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }

                var existingCourse = _context.Courses
                    //.Include(c => c.Chapters)
                    //    .ThenInclude(ch => ch.Lessons)
                    //        .ThenInclude(l => l.TheoryQuestions)
                    //.Include(c => c.Chapters)
                    //    .ThenInclude(ch => ch.PracticeQuestions)
                    //        .ThenInclude(cq => cq.TestCases)
                    //.Include(c => c.Chapters)
                    //    .ThenInclude(ch => ch.Lessons)
                    //        .ThenInclude(l => l.TheoryQuestions)
                    //          .ThenInclude(ans => ans.AnswerOptions)
                    //.Include(c => c.Chapters)
                    //    .ThenInclude(ch => ch.PracticeQuestions)
                    //            .ThenInclude(cq => cq.TestCases)
                    //.Include(c => c.Chapters)
                    //    .ThenInclude(ch => ch.PracticeQuestions)
                    //            .ThenInclude(cq => cq.UserAnswerCodes)
                        .FirstOrDefault(course => course.Id == request.Id);


                // Check if the course exist
                if (existingCourse == null)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }

                existingCourse.Name = request.Name;
                existingCourse.Description = request.Description;
                existingCourse.Picture = request.Picture;
                existingCourse.Tag = request.Tag;
                existingCourse.CreatedAt = request.CreatedAt;


                //foreach (var chapterDto in request.Chapters)
                //{
                //    var existingChapter = existingCourse.Chapters.FirstOrDefault(ch => ch.CourseId == chapterDto.Id);
                //    if (existingChapter != null)
                //    {
                //        existingChapter.Name = chapterDto.Name;
                //        existingChapter.Part = chapterDto.Part;
                //        existingChapter.IsNew = chapterDto.IsNew;
                //    }
                //    else
                //    {
                //        var newChapter = new Chapter
                //        {
                //            Name = chapterDto.Name,
                //            Part = chapterDto.Part,
                //            IsNew = chapterDto.IsNew
                //        };
                //        existingCourse.Chapters.Add(newChapter);
                //    }
                //    foreach (var codequestionDto in chapterDto.CodeQuestions)
                //    {
                //        var existCodeQuestion = existingChapter.PracticeQuestions.FirstOrDefault(code => code.Id == codequestionDto.Id);
                //        if (existCodeQuestion != null)
                //        {
                //            existCodeQuestion.Description = codequestionDto.Description;
                //        }
                //        else
                //        {
                //            var newCodeQuestion = new PracticeQuestion
                //            {
                //                ChapterId = existCodeQuestion.ChapterId,
                //                Description = codequestionDto.Description,
                //            };

                //            _context.PracticeQuestions.Add(newCodeQuestion);

                //        }

                //        foreach (var testcaseDto in codequestionDto.TestCases)
                //        {
                //            var existTestCase = existCodeQuestion.TestCases.FirstOrDefault(test => test.Id == testcaseDto.Id);
                //            if (existTestCase != null)
                //            {
                //                existTestCase.ExpectedResultInt = testcaseDto.ExpectedResultInt;
                //                existTestCase.ExpectedResultBoolean = testcaseDto.ExpectedResultBoolean;
                //                existTestCase.ExpectedResultString = testcaseDto.ExpectedResultString;
                //                existTestCase.InputTypeArrayInt = testcaseDto.InputTypeArrayInt;
                //                existTestCase.InputTypeArrayString = testcaseDto.InputTypeArrayString;
                //                existTestCase.InputTypeBoolean = testcaseDto.InputTypeBoolean;
                //                existTestCase.InputTypeString = testcaseDto.InputTypeString;
                //                existTestCase.InputTypeInt = testcaseDto.InputTypeInt;
                //                existTestCase.Id = testcaseDto.Id;
                //            }
                //            var newTestCase = new TestCase
                //            {
                //                InputTypeInt = testcaseDto.InputTypeInt,
                //                CodeQuestionId = existTestCase.CodeQuestionId,
                //                ExpectedResultString = testcaseDto.ExpectedResultString,
                //                InputTypeArrayInt = testcaseDto.InputTypeArrayInt,
                //                InputTypeArrayString = testcaseDto.InputTypeArrayString,
                //                ExpectedResultInt = testcaseDto.ExpectedResultInt,
                //                ExpectedResultBoolean = testcaseDto.ExpectedResultBoolean,
                //                InputTypeBoolean = testcaseDto.ExpectedResultBoolean,
                //                InputTypeString = testcaseDto.InputTypeString
                //            };
                //            _context.TestCases.Add(newTestCase);
                //        }

                //    }

                //    foreach (var lessonDto in chapterDto.Lessons)
                //    {
                //        var existingLesson = existingChapter.Lessons.FirstOrDefault(les => les.Id == lessonDto.Id);
                //        if (existingLesson != null)
                //        {

                //            existingLesson.Title = lessonDto.Title;
                //            existingLesson.VideoUrl = lessonDto.VideoUrl;
                //            existingLesson.Description = lessonDto.Description;
                //            existingLesson.Duration = lessonDto.Duration;
                //            existingLesson.ContentLesson= lessonDto.ContentLesson;
                //        }
                //        else
                //        {

                //            var newLesson = new Lesson
                //            {
                //                Title = lessonDto.Title,
                //                VideoUrl = lessonDto.VideoUrl,
                //                Description = lessonDto.Description,
                //                Duration = lessonDto.Duration,
                //                ContentLesson=lessonDto.ContentLesson
                //            };
                //            existingChapter.Lessons.Add(newLesson);
                //        }
                //        foreach (var questionDto in lessonDto.Questions)
                //        {
                //            var existingQuestion = existingLesson.TheoryQuestions.FirstOrDefault(q => q.Id == questionDto.Id);
                //            if (existingQuestion != null)
                //            {

                //                existingQuestion.ContentQuestion = questionDto.ContentQuestion;
                //                existingQuestion.Time = questionDto.Time;
                //            }
                //            else
                //            {
                //                var newQuestion = new TheoryQuestion
                //                {
                //                    ContentQuestion = questionDto.ContentQuestion,
                                 
                //                    Time = questionDto.Time
                //                };
                //                existingLesson.TheoryQuestions.Add(newQuestion);
                //            }

  

                //            foreach(var answerOptions in questionDto.AnswerOptions)
                //            {
                //                var existing = existingQuestion.AnswerOptions.FirstOrDefault(q=>q.Id == answerOptions.Id);

                //                if(existing != null)
                //                {
                //                    existing.OptionsText=answerOptions.OptionsText;
                //                    existing.CorrectAnswer = answerOptions.CorrectAnswer;
                //                }
                //                else
                //                {
                //                    var newAnswerOption = new AnswerOption
                //                    {
                //                        OptionsText = answerOptions.OptionsText,
                //                        QuestionId= questionDto.Id,
                //                        CorrectAnswer= answerOptions.CorrectAnswer
                //                    };
                //                    existingQuestion.AnswerOptions.Add(newAnswerOption);
                //                }
                //            }
                            
                           
                //        }
                //    }
                //}
                //var querry = await _context.Moderations.FirstOrDefaultAsync(c => c.CourseId.Equals(existingCourse.Id));
                //if (courseId != null)
                //{
                //    if (querry == null)
                //    {

                //        var moderation = new Moderation
                //        {
                //            CourseId = existingCourse.Id,
                //            CourseName = existingCourse.Name,
                //            ChangeType = "Modified",
                //            CreatedBy = existingCourse.CreatedBy,
                //            ApprovedContent = "Modified the course",
                //            Status = "Pending",
                //            CreatedAt = existingCourse.CreatedAt,
                            
                //        };
                //        await _context.Moderations.AddAsync(moderation);
                //    }

                //}
                //else
                //{
                //    querry.ChangeType = "Add";
                //    querry.CreatedAt = DateTime.Now;
                //}


                await _context.SaveChangesAsync(cancellationToken);
                return new OkObjectResult(existingCourse);
            }
        }
    }
}
