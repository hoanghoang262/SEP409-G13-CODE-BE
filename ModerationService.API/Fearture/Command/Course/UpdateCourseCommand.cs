using CourseService.API.Common.ModelDTO;
using MassTransit.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class UpdateCourseCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ChapterDTO> Chapters { get; set; }

        public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, IActionResult>
        {
            private readonly Content_ModerationContext _context;
            private readonly GetCourseIdGrpcServices services;

            public UpdateCourseCommandHandler(Content_ModerationContext context, GetCourseIdGrpcServices _services)
            {
                _context = context;
                services = _services;
            }

            public async Task<IActionResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
            {
                var courseId = await services.SendCourseId(request.Id);
                var existingCourse = _context.Courses
                        .Include(course => course.Chapters)
                        .ThenInclude(chapter => chapter.Lessons)
                        .ThenInclude(lesson => lesson.Questions)
                        .Include(course => course.Chapters)
                        .ThenInclude(chapter => chapter.CodeQuestions)
                        .ThenInclude(codeQuestion => codeQuestion.TestCases)
                        .FirstOrDefault(course => course.Id == request.Id);
                if (existingCourse == null)
                {
                    return new NotFoundResult();
                }


                existingCourse.Name = request.Name;
                existingCourse.Description = request.Description;
                existingCourse.Picture = request.Picture;
                existingCourse.Tag = request.Tag;
                existingCourse.CreatedBy = request.CreatedBy;


                foreach (var chapterDto in request.Chapters)
                {
                    var existingChapter = existingCourse.Chapters.FirstOrDefault(ch => ch.CourseId == chapterDto.Id);
                    if (existingChapter != null)
                    {
                        existingChapter.Name = chapterDto.Name;
                        existingChapter.Part = chapterDto.Part;
                        existingChapter.IsNew = chapterDto.IsNew;
                    }
                    else
                    {
                        var newChapter = new Chapter
                        {
                            Name = chapterDto.Name,
                            Part = chapterDto.Part,
                            IsNew = chapterDto.IsNew
                        };
                        existingCourse.Chapters.Add(newChapter);
                    }
                    foreach (var codequestionDto in chapterDto.CodeQuestions)
                    {
                        var existCodeQuestion = existingChapter.CodeQuestions.FirstOrDefault(code => code.Id == codequestionDto.Id);
                        if (existCodeQuestion != null)
                        {
                            existCodeQuestion.Description = codequestionDto.Description;
                        }
                        else
                        {
                            var newCodeQuestion = new CodeQuestion
                            {
                                ChapterId = existCodeQuestion.ChapterId,
                                Description = codequestionDto.Description,
                            };

                            _context.CodeQuestions.Add(newCodeQuestion);

                        }

                        foreach (var testcaseDto in codequestionDto.TestCases)
                        {
                            var existTestCase = existCodeQuestion.TestCases.FirstOrDefault(test => test.Id == testcaseDto.Id);
                            if (existTestCase != null)
                            {
                                existTestCase.ExpectedResultInt = testcaseDto.ExpectedResultInt;
                                existTestCase.ExpectedResultBoolean = testcaseDto.ExpectedResultBoolean;
                                existTestCase.ExpectedResultString = testcaseDto.ExpectedResultString;
                                existTestCase.InputTypeArrayInt = testcaseDto.InputTypeArrayInt;
                                existTestCase.InputTypeArrayString = testcaseDto.InputTypeArrayString;
                                existTestCase.InputTypeBoolean = testcaseDto.InputTypeBoolean;
                                existTestCase.InputTypeString = testcaseDto.InputTypeString;
                                existTestCase.InputTypeInt = testcaseDto.InputTypeInt;
                                existTestCase.Id = testcaseDto.Id;
                            }
                            var newTestCase = new TestCase
                            {
                                InputTypeInt = testcaseDto.InputTypeInt,
                                CodeQuestionId = existTestCase.CodeQuestionId,
                                ExpectedResultString = testcaseDto.ExpectedResultString,
                                InputTypeArrayInt = testcaseDto.InputTypeArrayInt,
                                InputTypeArrayString = testcaseDto.InputTypeArrayString,
                                ExpectedResultInt = testcaseDto.ExpectedResultInt,
                                ExpectedResultBoolean = testcaseDto.ExpectedResultBoolean,
                                InputTypeBoolean = testcaseDto.ExpectedResultBoolean,
                                InputTypeString = testcaseDto.InputTypeString
                            };
                            _context.TestCases.Add(newTestCase);
                        }

                    }

                    foreach (var lessonDto in chapterDto.Lessons)
                    {
                        var existingLesson = existingChapter.Lessons.FirstOrDefault(les => les.Id == lessonDto.Id);
                        if (existingLesson != null)
                        {

                            existingLesson.Title = lessonDto.Title;
                            existingLesson.VideoUrl = lessonDto.VideoUrl;
                            existingLesson.Description = lessonDto.Description;
                            existingLesson.Duration = lessonDto.Duration;
                        }
                        else
                        {

                            var newLesson = new Lesson
                            {
                                Title = lessonDto.Title,
                                VideoUrl = lessonDto.VideoUrl,
                                Description = lessonDto.Description,
                                Duration = lessonDto.Duration
                            };
                            existingChapter.Lessons.Add(newLesson);
                        }

                        foreach (var questionDto in lessonDto.Questions)
                        {
                            var existingQuestion = existingLesson.Questions.FirstOrDefault(q => q.Id == questionDto.Id);
                            if (existingQuestion != null)
                            {

                                existingQuestion.ContentQuestion = questionDto.ContentQuestion;
                                existingQuestion.AnswerA = questionDto.AnswerA;
                                existingQuestion.AnswerB = questionDto.AnswerB;
                                existingQuestion.AnswerC = questionDto.AnswerC;
                                existingQuestion.AnswerD = questionDto.AnswerD;
                                existingQuestion.CorrectAnswer = questionDto.CorrectAnswer;
                                existingQuestion.Time = questionDto.Time;
                            }
                            else
                            {

                                var newQuestion = new Question
                                {
                                    ContentQuestion = questionDto.ContentQuestion,
                                    AnswerA = questionDto.AnswerA,
                                    AnswerB = questionDto.AnswerB,
                                    AnswerC = questionDto.AnswerC,
                                    AnswerD = questionDto.AnswerD,
                                    CorrectAnswer = questionDto.CorrectAnswer,
                                    Time = questionDto.Time
                                };
                                existingLesson.Questions.Add(newQuestion);
                            }
                        }
                    }
                }
                var querry = await _context.Moderations.FirstOrDefaultAsync(c => c.CourseId.Equals(existingCourse.Id));
                if (courseId != null)
                {
                    if (querry == null)
                    {
                        var moderation = new Moderation
                        {
                            CourseId = existingCourse.Id,
                            CourseName = existingCourse.Name,
                            ChangeType = "Modified",
                            CreatedBy = existingCourse.Name,
                            ApprovedContent = "Modified the course",
                            Status = "Pending",
                            CreatedAt = existingCourse.CreatedAt
                        };
                        await _context.Moderations.AddAsync(moderation);
                    }

                }
                else
                {
                    querry.ChangeType = "Add";
                    querry.CreatedAt = DateTime.Now;
                }


                await _context.SaveChangesAsync(cancellationToken);
                return new OkObjectResult("Your course has been updated successfully.");
            }
        }
    }
}
