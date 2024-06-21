using Eduology.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Eduology.Infrastructure.Persistence;
using Eduology.Domain.Interfaces;
using Eduology.Infrastructure.Repositories;
using Eduology.Application.Services;
using Eduology.Application.Services.Interface;
using Eduology.Infrastructure.Services;
using Eduology.Application.Interface;
using Microsoft.IdentityModel.Tokens;
using Eduology.Application.Services.Helper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            /////////////////
            // Add services to the container.
            builder.Services.Configure<JWT>(Configuration.GetSection("JWT"));

            // Add Entity Framework Core DbContext
            builder.Services.AddDbContext<EduologyDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<EduologyDBContext>();

            // Register repositories
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            // Register services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IAsignmentServices, AssignmentServices>();
            builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
            builder.Services.AddScoped<IOrganizationService, OrganizationService>();
            builder.Services.AddScoped<ISubmissionService, SubmissionService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<IModuleService, ModuleServicecs>();
            builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
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
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("InstructorPolicy", policy => policy.RequireRole("Instructor"));
                options.AddPolicy("StudentPolicy", policy => policy.RequireRole("Student"));
            });
        
        // Add controllers and Swagger
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