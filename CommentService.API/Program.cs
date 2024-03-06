using GrpcServices;
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
            builder.Services.AddScoped<GetUserPostGrpcService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapControllerRoute(
                name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
