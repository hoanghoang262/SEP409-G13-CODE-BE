using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.API.MessageBroker;
using NotificationService.API.Models;
using System.Reflection;

namespace NotificationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<NotificationContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //rabbitMQ
            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;

            var mqConnection = new Uri(configuration);

            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<EventNotificationHandler>();
                

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    cfg.ConfigureEndpoints(ctx);

                });

            });
            //mapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //mediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //grpc
            //var config = builder.Configuration.GetSection("GrpcSetting:CourseUrl").Value;
            //builder.Services.AddSingleton(config);
            //builder.Services.AddGrpcClient<UserCourseService.UserCourseServiceClient>(x => x.Address = new Uri(config));
            //builder.Services.AddScoped<UserIdCourseGrpcService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
