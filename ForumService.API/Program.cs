using ForumService.API.Fearture.Command;
using ForumService.API.MessageBroker;
using ForumService.API.Models;
using GrpcServices;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserGrpc;


namespace ForumService.API
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
            //automaper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // rabbitMQ
            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;

            var mqConnection = new Uri(configuration);

            builder.Services.AddMassTransit(config =>
            {
              
                config.AddConsumersFromNamespaceContaining<EventPostHandler>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    cfg.ConfigureEndpoints(ctx);

                });

            });

            //gRPC
            var config = builder.Configuration.GetSection("GrpcSetting:UserUrl").Value;
            builder.Services.AddSingleton(config);
            builder.Services.AddGrpcClient<GetUserService.GetUserServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<GetUserPostGrpcService>();


            builder.Services.AddDbContext<ForumContext>(
  options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
  );         //mediator
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllerRoute(
                 name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
