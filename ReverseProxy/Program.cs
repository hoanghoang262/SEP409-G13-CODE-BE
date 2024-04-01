using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using System.Text;

namespace ReverseProxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            //builder.Services.AddReverseProxy()

            //    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxyAccount"))
            //    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxyAuthen"));
            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("customPolicy", policy =>
            //        policy.RequireAuthenticatedUser());
            //});

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>

            {
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath).AddJsonFile("ocelot.json",optional:false,reloadOnChange:true).AddEnvironmentVariables();
            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();
          //  app.UseHttpsRedirection();


     
          
            app.UseAuthorization();

            app.UseAuthentication();
           
            app.MapControllers();
         

            //app.UseSwaggerForOcelotUI(opt =>
            //{
            //    opt.PathToSwaggerGenerator = "/swagger/docs";
            //});
            app.UseOcelot();

            // app.MapReverseProxy();

            app.Run();
        }
    }
}