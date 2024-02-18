using AutoMapper;
using CourseGRPC.Services;
using EventBus.Message.IntegrationEvent.PublishEvent;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Common.PublishEvent;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Moderation
{
    public class ModerationCourseCommand : IRequest<IActionResult>
    {
        public int CourseId { get; set; }

        public class ModerationCourseCommandHandler : IRequestHandler<ModerationCourseCommand, IActionResult>
        {
            private readonly IPublishEndpoint _publish;
            private readonly Content_ModerationContext _context;
            private readonly IMapper _mapper;
            private readonly UserEnrollCourseGrpcServices _services;
            public ModerationCourseCommandHandler(IPublishEndpoint publish, Content_ModerationContext context,IMapper mapper,UserEnrollCourseGrpcServices service)
            {
                _context = context;
                _publish = publish;
                _mapper = mapper;
                _services=service;

            }
            public async Task<IActionResult> Handle(ModerationCourseCommand request, CancellationToken cancellationToken)
            {
                var userId=_services.SendCourseId(request.CourseId);
                var course =  await  _context.Courses.FirstOrDefaultAsync(c => c.Id.Equals(request.CourseId));
                var courseEvent = new CourseEvent
                {
                    Id=course.Id,
                    Name = course.Name,
                    CreatedBy = course.CreatedBy,
                    Description = course.Description,
                    Tag = course.Tag,
                    Picture = course.Picture,
                    CreatedAt = course.CreatedAt,
                };  
                await _publish.Publish(courseEvent);

                var chapter = await _context.Chapters.Where(c => c.CourseId.Equals(request.CourseId)).ToListAsync();
                foreach (var chap in chapter)
                {
                    var chapterEvent = new ChapterEvent
                    {
                        Id = chap.Id,
                        Name = chap.Name,
                        IsNew = chap.IsNew,
                        CourseId = chap.CourseId,
                        Part = chap.Part
                    };
                    await _publish.Publish(chapterEvent);
                    var lesson = await _context.Lessons.Where(l => l.ChapterId.Equals(chap.Id)).ToListAsync();
                    foreach (var less in lesson)
                    {
                        var lessonEvent = new LessonEvent
                        {
                            Id = less.Id,
                            ChapterId = chap.Id,
                            Description = less.Description,
                            VideoUrl = less.VideoUrl,
                            Title = less.Title,
                            Duration = less.Duration,
                            IsCompleted=false
                            
                        };
                        await _publish.Publish(lessonEvent);
                        var question = await _context.Questions.Where(q => q.VideoId.Equals(less.Id)).ToListAsync();
                        foreach (var ques in question)
                        {
                            var questionEvent = new QuestionEvent
                            {
                                Id = ques.Id,
                                AnswerA = ques.AnswerA,
                                AnswerB = ques.AnswerB,
                                AnswerC = ques.AnswerC,
                                AnswerD = ques.AnswerD,
                                ContentQuestion = ques.ContentQuestion,
                                CorrectAnswer = ques.CorrectAnswer,
                                Time = ques.Time,
                                VideoId = less.Id
                            };
                            await _publish.Publish(questionEvent);
                        }
                    }
                }
                if (userId != null)
                {
                    foreach (var id in userId.Result.UserId)
                    {
                        var notification = new NotificationEvent
                        {
                            RecipientId = id,
                            IsSeen = false,
                            NotificationContent = "Your course have been change. Please check the new content",
                            SendDate = DateTime.Now,
                            Course_Id = course.Id,
                        };
                        await _publish.Publish(notification);
                    }

                }
                var notificationForAdminBussiness= new NotificationEvent
                {
                    RecipientId = course.CreatedBy,
                    IsSeen = false,
                    NotificationContent = "Your course has been approved",
                    SendDate = DateTime.Now,
                    Course_Id=course.Id,
                };
                await _publish.Publish(notificationForAdminBussiness);



                return new OkObjectResult("async success");
            }
        }
    }
}
