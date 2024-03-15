
using CourseGRPC;
using CourseGRPC.Services;
using EventBus.Message.Event;
using GrpcServices;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.GrpcServices;
using ModerationService.API.Models;
using Serilog;
using System.Reflection;
using UserGrpc;

namespace ModerationService.API
{

    public class Program
    {
        public static void Main(string[] args)
        {
           
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddDbContext<Content_ModerationContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //rabbitMQ
            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;
            var mqConnection = new Uri(configuration);
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
                config.AddRequestClient<CourseEvent>();
            });

            //cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });
            //gRPC
            var config = builder.Configuration.GetSection("GrpcSetting:UserUrl").Value;
            builder.Services.AddSingleton(config);
            builder.Services.AddGrpcClient<UserCourseService.UserCourseServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<UserIdCourseGrpcService>();

            builder.Services.AddGrpcClient<GetUserService.GetUserServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<GetUserInfoService>();

            var config2 = builder.Configuration.GetSection("GrpcSetting2:CourseUrl").Value;
            builder.Services.AddGrpcClient<UserEnrollCourseService.UserEnrollCourseServiceClient>(x => x.Address = new Uri(config2));
            builder.Services.AddScoped<UserEnrollCourseGrpcServices>();

            builder.Services.AddGrpcClient<GetCourseByIdService.GetCourseByIdServiceClient>(x => x.Address = new Uri(config2));
            builder.Services.AddScoped<GetCourseIdGrpcServices>();

            builder.Services.AddGrpcClient<CheckCourseIdService.CheckCourseIdServiceClient>(x => x.Address = new Uri(config2));
            builder.Services.AddScoped<CheckCourseIdServicesGrpc>();
            //mapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //mediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
           
                app.UseSwagger();
                app.UseSwaggerUI();
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();
            app.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");


            app.MapControllers();

            app.Run();
        }
    }
}
