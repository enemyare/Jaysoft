using System.Reflection;
using Asp.Versioning;
using FluentValidation.AspNetCore;
using MerosWebApi.Persistence;
using MerosWebApi.Application;
using MerosWebApi.Application.Common.DTOs.UserService.DtoValidators;
using MerosWebApi.ForSwagger;
using Microsoft.OpenApi.Models;

namespace MerosWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var configuration = builder.Configuration;

            builder.Services.AddDataAccess(configuration);
            builder.Services.AddDevEmailConfiguration(configuration);
            builder.Services.AddAppSettings(configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddSecurityServices(configuration);

            builder.Services.AddControllersAndFluentValidatation(configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Cookie", new OpenApiSecurityScheme
                {
                    Name = "Cookie",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Description = "Cookie-based authentication. \r\n\r\n" +
                                  "Ensure to include the authentication cookie in your requests. \r\n\r\n" +
                                  "Example: \"authToken = ......\""
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Cookie"
                            }
                        },
                        new string[] {}
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);

                swagger.SupportNonNullableReferenceTypes();
            });

            builder.Services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("api-version"));
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {

                });
            }

            app.UseCors(policy => 
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                );

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
