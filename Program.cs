using api.Db;
using api.Services.AuthService;
using api.Services.SampleService;
using api.Services.SignedUrl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<ZvuchokContext>(options => options.UseSqlite("Data Source=context.db"));

            builder.Services.AddScoped<ISampleService, SampleService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ISignedUrlService, SignedUrlService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["AppSettings:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]))
                    };
                });

            var app = builder.Build();

            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
