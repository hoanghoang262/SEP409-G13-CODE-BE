using Authenticate_Service;
using Authenticate_Service.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Contract.Service.Configuration;
using Contract.Service;
using Microsoft.OpenApi.Models;




namespace Authenticated
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();

            //config RabbitMQ
            var configuration = builder.Configuration.GetSection("EventBusSetting:HostAddress").Value;

            var mqConnection = new Uri(configuration);

            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
                //config.AddRequestClient<CourseMessage>();

            });
            //gRPC
            var config = builder.Configuration.GetSection("GrpcSetting:UserUrl").Value;
            builder.Services.AddSingleton(config);
            // builder.Services.AddGrpcClient<CourseMessage>(x=>x.Address = new Uri(config));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin());
            });
            //Config email
            var email = builder.Configuration.GetSection(nameof(SmtpEmailSetting)).Get<SmtpEmailSetting>();
            builder.Services.AddSingleton(email);
            builder.Services.AddScoped<IEmailService<MailRequest>, SMTPEmailService>();
            //Config autoMapper
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            //Config MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            builder.Services.AddRazorPages();
            //Config context
            builder.Services.AddDbContext<AuthenticationContext>(
    oprions => oprions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

            //Config Jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer token for JWT Authorization",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };
                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        new string[] {}
                    }
                };
                opt.AddSecurityRequirement(securityRequirement);
            });

            //builder.Services.AddHttpContextAccessor();

            var app = builder.Build();


            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            IConfiguration configuration1 = app.Configuration;
            IWebHostEnvironment environment = app.Environment;

            app.UseCors("AllowSpecificOrigin");
            app.MapRazorPages();

            app.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllers();


            app.Run();
        }
    }
}