using FluentValidation.AspNetCore;
using MerosWebApi.Persistence;
using MerosWebApi.Application;
using MerosWebApi.Application.Common.DTOs.UserService.DtoValidators;

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
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
