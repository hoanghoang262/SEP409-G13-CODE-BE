using CloudinaryDotNet;

namespace Image
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
            Account cloudinaryConfig = new Account(
                  "dcduktpij",
                  "592561579458269",
                  "rriM4lqd8uNQ9FtUd11NjTq50ac"
                );

            Cloudinary cloudinary = new Cloudinary(cloudinaryConfig);
            cloudinary.Api.Secure = true;
            builder.Services.AddSingleton(cloudinary);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}