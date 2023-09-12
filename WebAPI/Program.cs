
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using WebAPI.AppDtos.ConfigModels;
using WebAPI.Controllers.Auth;
using WebAPI.Controllers.Countries;
using WebAPI.Controllers.Event;
using WebAPI.Controllers.Trainers;
using WebAPI.Controllers.User;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.StandaloneServices.CertificateBuilderService;
using WebAPI.StandaloneServices.EmailService;
using WebAPI.StandaloneServices.EmailServiceNamespace;
using WebAPI.StandaloneServices.MailConfigModels;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Swagger configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                      new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference
                      {
                         Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                      }
                         },
                         new string[] { }
                    }
                 });

            });

            //CORS configuration
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                {
                    options.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            //Mail service parameters configuration
            builder.Services.Configure<MailServiceSettings>(
                builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<AuthConfig>(
                builder.Configuration.GetSection("JWT"));

            // For Identity  
            builder.Services.AddIdentity<User,IdentityRole>()
                .AddEntityFrameworkStores<MainContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidAudience = Configuration["JWT:ValidAudience"],
                    //ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });

            //Database context configuration
            builder.Services.AddDbContext<MainContext>(options =>
                options.UseNpgsql("Host=db;Port=5432;Database=postgres;Username=admin;Password=*9797*SiB@Proj!"));

            /**
             * Application services configuration
             */
            //Auth Config:
            builder.Services.AddScoped<AuthService>();
            //Event Config:
            builder.Services.AddScoped<EventService>();
            //Email Config:
            builder.Services.AddScoped<EmailService>();
            //Country Config:
            builder.Services.AddScoped<CountryService>();
            //Trainer Config:
            builder.Services.AddScoped<TrainerService>();
            //User Config
            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped<CertificateBuilderService>();

            var app = builder.Build();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //if (builder.Configuration.GetSection("MigrateDatabaseConfig").Get<MigrateDatabaseConfig>().MigrateDb)
            //{
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        //Dropping current database schema.
            //        var dbContext = scope.ServiceProvider
            //            .GetRequiredService<MainContext>();
            //        dbContext.Database.ExecuteSql(@$"drop schema public cascade;create schema public;");
                    
            //        // Executing migration scripts.
            //        dbContext.Database.Migrate();
            //    }
            //}
            

            app.UseCors("AllowOrigin");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}