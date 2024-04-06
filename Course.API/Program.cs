
using CloudinaryDotNet;
using CompileCodeOnline;
using CourseService.API.Controllers;
using CourseService.API.GrpcServices;
using CourseService.API.MessageBroker;
using CourseService.API.Models;
using GrpcServices;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserGrpc;

namespace CourseService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<DynamicCodeCompiler>();
          
            

            // rabbitMQ
            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;

            var mqConnection = new Uri(configuration);

            builder.Services.AddMassTransit(config =>
            {
                //config.AddConsumersFromNamespaceContaining<EventCourseIdHandler>();
                config.AddConsumersFromNamespaceContaining<EventCourseHandler>();
                config.AddConsumersFromNamespaceContaining<EventChapterHandler>();
                config.AddConsumersFromNamespaceContaining<EventLessonHandler>();
                config.AddConsumersFromNamespaceContaining<EventTheoryQuestionHandler>();
                config.AddConsumersFromNamespaceContaining<EventPracticeQuestionHandler>();
                config.AddConsumersFromNamespaceContaining<EventAnswerOptionsHandler>();
                config.AddConsumersFromNamespaceContaining<EventLastExamHandler>();
                config.AddConsumersFromNamespaceContaining<EventExamHandler>();
                config.AddConsumersFromNamespaceContaining<EventExamAnswerHandler>();
                config.AddConsumersFromNamespaceContaining<EventUserEnrollHandler>();



                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    cfg.ConfigureEndpoints(ctx);
                    
                });

            });
          

        var cloudinaryConfig = new Account(
        builder.Configuration["Cloudinary:CloudName"],
        builder.Configuration["Cloudinary:ApiKey"],
        builder.Configuration["Cloudinary:ApiSecret"]
    );

            var cloudinary = new Cloudinary(cloudinaryConfig);
            builder.Services.AddSingleton(cloudinary);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin());
            });

            //grpc
            var config = builder.Configuration.GetSection("GrpcSetting:UserUrl").Value;
            builder.Services.AddSingleton(config);
            builder.Services.AddGrpcClient<UserCourseService.UserCourseServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<UserIdCourseGrpcService>();

            builder.Services.AddGrpcClient<GetUserService.GetUserServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<GetUserInfoService>();
            //dbContext
            builder.Services.AddDbContext<CourseContext>(
    oprions => oprions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );      //mapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //mediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //Cloudinary
           
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
           
                app.UseSwagger();
                app.UseSwaggerUI();
        

            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");

            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");



            app.MapControllers();

            app.Run();
        }
    }
}
