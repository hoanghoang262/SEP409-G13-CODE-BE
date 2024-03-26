using CommentService.API.Models;
using ForumService.API.MessageBroker;
using GrpcServices;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserGrpc;

namespace CommentService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var config = builder.Configuration.GetSection("GrpcSetting:UserUrl").Value;
            builder.Services.AddSingleton(config);
            builder.Services.AddGrpcClient<GetUserService.GetUserServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<GetUserInfoGrpcService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            builder.Services.AddDbContext<CommentContext>(
  oprions => oprions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
  );
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;

            var mqConnection = new Uri(configuration);

            builder.Services.AddMassTransit(config =>
            {

                config.AddConsumersFromNamespaceContaining<EventPostIdHandler>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    cfg.ConfigureEndpoints(ctx);

                });

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapControllerRoute(
                name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");


            app.MapControllers();

            app.Run();
        }
    }
}
