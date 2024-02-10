using CourseService.API.Common.ModelDTO;
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
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }

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
                    return new NotFoundResult(); // Trả về mã lỗi 404 nếu không tìm thấy khóa học
                }

                // Cập nhật thông tin của khóa học
                existingCourse.Name = request.Name;
                existingCourse.Description = request.Description;
                existingCourse.Picture = request.Picture;
                existingCourse.Tag = request.Tag;
                existingCourse.CreatedBy = request.UserId;

                // Xóa tất cả các chương, bài học và câu hỏi hiện có liên quan đến khóa học
                _context.Questions.RemoveRange(existingCourse.Chapters.SelectMany(ch => ch.Lessons.SelectMany(les => les.Questions)));
                _context.Lessons.RemoveRange(existingCourse.Chapters.SelectMany(ch => ch.Lessons));
                _context.Chapters.RemoveRange(existingCourse.Chapters);

                // Thêm các chương, bài học và câu hỏi mới
                foreach (var chapterDto in request.Chapters)
                {
                    var newChapter = new Chapter
                    {
                        Name = chapterDto.Name,
                        Part = chapterDto.Part,
                        IsNew = chapterDto.IsNew
                    };

                    existingCourse.Chapters.Add(newChapter);

                    foreach (var lessonDto in chapterDto.Lessons)
                    {
                        var newLesson = new Lesson
                        {
                            Title = lessonDto.Title,
                            VideoUrl = lessonDto.VideoUrl,
                            Description = lessonDto.Description,
                            Duration = lessonDto.Duration
                        };

                        newChapter.Lessons.Add(newLesson);

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
                                Time = questionDto.Time
                            };

                            newLesson.Questions.Add(newQuestion);
                        }
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult("Your course has been updated successfully.");
            }
        }
    }
}
