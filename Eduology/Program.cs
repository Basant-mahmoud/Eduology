using Eduology.Domain.Models;
//using Eduology.Infrastructure.Persistence
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Eduology.Infrastructure.Persistence;
using Eduology.Infrastructure.Extensions;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using Eduology.Application.Services;
using Eduology.Application.Services.Interface;
using Eduology.Infrastructure.Services_class;
using Eduology.Application.Services.Helper;
using Eduology.Infrastructure.Services;
using Eduology.Application.Interface;
namespace Eduology
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Build the IConfiguration instance
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWebApp",
                    policyBuilder => policyBuilder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod().
                        AllowCredentials());
            });

            // Add services to the container.
            builder.Services.Configure<JWT>(Configuration.GetSection("JWT"));
            // Add Entity Framework Core DbContext
            builder.Services.AddInfrastructure(Configuration);
            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<EduologyDBContext>();
            // Add Services of the role
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IAsignmentServices, AssignmentServices>();
            //Add configuration of JWT Service
            builder.Services.AddAuthentication(options =>
            {
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer( o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                });
            builder.Services.Configure<JWT>(Configuration.GetSection("JWT"));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseCors("AllowWebApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
