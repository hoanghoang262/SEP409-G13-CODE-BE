



using CloudinaryDotNet;
using CourseService;
using CourseService.API;
using CourseService.API.IntegrationEvent.EvenHandles;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Course
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;

            var mqConnection = new Uri(configuration);

            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<EventHanler>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    cfg.ReceiveEndpoint("LoginEvent", c =>
                    {
                        c.ConfigureConsumer<EventHanler>(ctx);
                    });


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
            builder.Services.AddDbContext<CourseContext>(
    oprions => oprions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


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
