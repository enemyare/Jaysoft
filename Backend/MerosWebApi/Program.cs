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
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                                  "Input JWT string only " +
                                  "\r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im" +
                                  "M5Y2M1ZGM0LTg4NTktNDY4Yi04NDExLTFhZTMxYjYxODY5NyIsIm5iZiI6MTczMTA2NzIxNywiZXhwI" +
                                  "joxNzMxMDcwODE3LCJpYXQiOjE3MzEwNjcyMTd9.Gde5aETdODmv0HYHOh1a8CiX1UjTx3gTdev_9OJf49U\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
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

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
