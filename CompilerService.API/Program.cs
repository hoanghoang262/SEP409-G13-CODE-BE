using CompilerService.API.Controllers;
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
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin());
            });


            var app = builder.Build();

           
          
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
