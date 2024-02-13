using CourseService.API.Common.ModelDTO;
using MassTransit.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            public UpdateCourseCommandHandler(Content_ModerationContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
            {
                var existingCourse = await _context.Courses
                    .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                    .ThenInclude(les => les.Questions)
                    .FirstOrDefaultAsync(c => c.Id == request.Id);

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
                    var existingChapter = existingCourse.Chapters.FirstOrDefault(ch => ch.Id == chapterDto.Id);
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
                if (querry == null)
                {
                    var moderation = new Moderation
                    {
                        CourseId = existingCourse.Id,
                        ChangeType = "Modified",
                        CreatedBy = existingCourse.Name,
                        ApprovedContent = "Modified the course",
                        Status = "Pending",
                        CreatedAt = existingCourse.CreatedAt
                    };
                    await _context.Moderations.AddAsync(moderation);
                }
                else
                {
                    querry.CreatedAt = DateTime.Now;
                }
               

                await _context.SaveChangesAsync(cancellationToken);
                return new OkObjectResult("Your course has been updated successfully.");
            }
        }
    }
}
