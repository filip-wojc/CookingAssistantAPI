
using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Repositories;
using CookingAssistantAPI.Services;
using CookingAssistantAPI.Tools;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

            var authParameters = new JwtParameters();
            builder.Configuration.GetSection("Authentication").Bind(authParameters);

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logging.RequestBodyLogLimit = 4096; // limit the size of the request body to log
                logging.ResponseBodyLogLimit = 4096; // limit the size of the response body to log
                logging.MediaTypeOptions.AddText("application/json"); // specify media types to log
            });

            builder.Services.AddSingleton(authParameters);
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Bearer";
                o.DefaultScheme = "Bearer";
                o.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {

                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authParameters.JwtIssuer,
                    ValidAudience = authParameters.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authParameters.JwtKey)),
                };
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CookingAssistantAPI", Version = "v1" });
                c.OperationFilter<FileUploadOperationFilter>();

                // Konfiguracja JWT w Swaggerze
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
            }});
            });

            builder.Services.AddDbContext<CookingDbContext>(
                o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddExceptionHandler<AppExceptionHandler>();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();
            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseExceptionHandler(_ => { });

            app.UseHttpLogging();

            // Configure the HTTP request pipeline.
           
            app.UseSwagger();
            app.UseSwaggerUI();
            

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
