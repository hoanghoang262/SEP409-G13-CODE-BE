using CourseGRPC;
using CourseService.API.GrpcServices;
using GrpcServices;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.GrpcServices;
using PaymentService.Interface;
using PaymentService.Models;
using PaymentService.ServicePayment.VnPay;
using System.Reflection;
using UserGrpc;

namespace PaymentService
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
            builder.Services.AddDbContext<PaymentContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;
            var mqConnection = new Uri(configuration);
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
               
            });

            var config = builder.Configuration.GetSection("GrpcSetting:UserUrl").Value;
            builder.Services.AddSingleton(config);
            builder.Services.AddGrpcClient<UserCourseService.UserCourseServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<UserIdCourseGrpcService>();

            builder.Services.AddGrpcClient<GetUserService.GetUserServiceClient>(x => x.Address = new Uri(config));
            builder.Services.AddScoped<GetUserInfoService>();

            var config2 = builder.Configuration.GetSection("GrpcSetting2:CourseUrl").Value;
            builder.Services.AddGrpcClient<GetCourseByIdService.GetCourseByIdServiceClient>(x => x.Address = new Uri(config2));
            builder.Services.AddScoped<GetCourseInfoService>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin());
            });


            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.Configure<VnpayConfig>(
            builder.Configuration.GetSection(VnpayConfig.ConfigName));

            builder.Services.Configure<MomoConfig>(
             builder.Configuration.GetSection(MomoConfig.ConfigName));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            var app = builder.Build();


            // Configure the HTTP request pipeline.
           
                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");

            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
