using AutoMapper;
using CourseGRPC.Services;
using EventBus.Message.Event;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ModerationService.API.Models;
using System.Threading;

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
            public  async Task<IActionResult> Handle(ModerationCourseCommand request, CancellationToken cancellationToken)
            {
                var courseIdEvent = new CourseIdEvent
                {
                    CourseId = request.CourseId,
                };
                await _publish.Publish(courseIdEvent);
                var userId=_services.SendCourseId(request.CourseId);
               
                var course = _context.Courses.FirstOrDefault(c => c.Id.Equals(request.CourseId));
               
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
               await  _publish.Publish(courseEvent);
                
               
                var chapter =  _context.Chapters.Where(c => c.CourseId.Equals(request.CourseId)).ToList();
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
                    
                    var codequestion =  _context.PracticeQuestions.Where(c => c.ChapterId.Equals(chap.Id)).ToList();

                    foreach (var code in codequestion)
                    {
                        var codequestionEvent = new PracticeQuestionEvent
                        {
                            Description = code.Description,
                            ChapterId = code.ChapterId,
                            Id = code.Id,
                            CodeForm=code.CodeForm
                        };
                        await _publish.Publish(codequestionEvent);
                   
                        var testcase=  _context.TestCases.Where(c => c.CodeQuestionId.Equals(code.Id)).ToList();
                        foreach(var test in testcase)
                        {
                            var testcaseEvent = new TestCaseEvent
                            {
                                Id = test.Id,
                                InputTypeInt = test.InputTypeInt,
                                CodeQuestionId = test.CodeQuestionId,
                                ExpectedResultString = test.ExpectedResultString,
                                InputTypeArrayInt = test.InputTypeArrayInt,
                                InputTypeArrayString = test.InputTypeArrayString,
                                ExpectedResultInt = test.ExpectedResultInt,
                                ExpectedResultBoolean = test.ExpectedResultBoolean,
                                InputTypeBoolean = test.ExpectedResultBoolean,
                                InputTypeString = test.InputTypeString
                            };
                            await _publish.Publish(testcaseEvent);
                            
                        }
                        
                    }
                    
                    var lesson =  _context.Lessons.Where(l => l.ChapterId.Equals(chap.Id)).ToList();
                   
                    foreach (var less in lesson)
                    {
                        var lessonEvent = new LessonEvent
                        {
                            Id = less.Id,
                            ChapterId = less.ChapterId,
                            Description = less.Description,
                            VideoUrl = less.VideoUrl,
                            Title = less.Title,
                            Duration = less.Duration,
                            IsCompleted=false
                            
                        };
                        await _publish.Publish(lessonEvent);
                    
                        var question =  _context.TheoryQuestions.Where(q => q.VideoId.Equals(less.Id)).ToList();
                        foreach (var ques in question)
                        {
                            var questionEvent = new TheoryQuestionEvent
                            {
                                Id = ques.Id,
                                ContentQuestion = ques.ContentQuestion,
                                Time = ques.Time,
                                VideoId = ques.VideoId
                            };
                            await _publish.Publish(questionEvent);
                        
                            var answerOption =  _context.AnswerOptions.Where(q => q.QuestionId.Equals(ques.Id)).ToList();
                            foreach(var ansOptio in answerOption)
                            {
                                var ansOpEvent = new AnswerOptionsEvent
                                {
                                    Id = ansOptio.Id,
                                    QuestionId = ansOptio.QuestionId,
                                    OptionsText = ansOptio.OptionsText,
                                    CorrectAnswer=ansOptio.CorrectAnswer
                                };
                                await _publish.Publish(ansOpEvent);
                             
                            }
                           
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
                else
                {
                    var notificationForAdminBussiness = new NotificationEvent
                    {
                        RecipientId = course.CreatedBy,
                        IsSeen = false,
                        NotificationContent = "Your course has been approved",
                        SendDate = DateTime.Now,
                        Course_Id = course.Id,
                    };
                    await _publish.Publish(notificationForAdminBussiness);

                }
                




               return  new OkObjectResult("ok");
            }
        }
    }
}
