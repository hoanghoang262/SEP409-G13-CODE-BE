using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentService.Interface;
using PaymentService.Models;
using PaymentService.ServicePayment.VnPay;
using System.Reflection;

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


            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
