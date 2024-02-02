



using CloudinaryDotNet;
using Course.API;
using Course.API.GrpcServices;
using CourseService;

using CourseService.API.Feartures.CourseFearture.Queries;
using CourseService.API.IntegrationEvent.EvenHandles;
using EventBus.Message.IntegrationEvent.Event;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using User.gRPC;

namespace Course
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // rabbitMQ
            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;

            var mqConnection = new Uri(configuration);

            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<EventHanler>();

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
                    builder => builder.WithOrigins(new string[] { "http://localhost:5118" }) // Thay thế bằng nguồn gốc thực tế của bạn
                                        .AllowAnyMethod()
                                        .AllowAnyHeader());
            });
            //grpc
            var config = builder.Configuration.GetSection("GrpcSetting:UserUrl").Value;
            builder.Services.AddSingleton(config);
            builder.Services.AddGrpcClient<UserCourseService.UserCourseServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<UserIdCourseGrpcService>();
            //dbContext
            builder.Services.AddDbContext<CourseContext>(
    oprions => oprions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );      //mapper
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            //mediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //Cloudinary
            builder.Services.AddSingleton(new CloudinaryService("dcduktpij", "592561579458269", "rriM4lqd8uNQ9FtUd11NjTq50ac"));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
