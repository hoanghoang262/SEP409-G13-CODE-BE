using CompilerService.API.Models;
using CourseService.API.Controllers;
using Microsoft.EntityFrameworkCore;

namespace CompilerService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
          
            builder.Services.AddScoped<DynamicCodeCompilerJava>();
         
            builder.Services.AddScoped<CCompiler>();
            builder.Services.AddScoped<CPlushCompiler>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<CourseContext>(
  oprions => oprions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
  );

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
