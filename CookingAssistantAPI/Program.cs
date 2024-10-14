
using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Repositories;
using CookingAssistantAPI.Services;
using CookingAssistantAPI.Tools;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace CookingAssistantAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                o.JsonSerializerOptions.WriteIndented = true;
            });


            var repositoriesToRegister = new List<IRegistrationResource>()
            {
                new RepositoryRegistration(),
                new ServicesRegistration()
            };

            foreach (var resource in repositoriesToRegister)
            {
                resource.Register(builder.Services);
            }

            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CookingDbContext>(
                o => o.UseSqlite("Data Source=recipes.db")
                );

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddExceptionHandler<AppExceptionHandler>();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseExceptionHandler(_ => { });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
