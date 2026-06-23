using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TechMoveGLMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Gets the connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Register the ApplicationDbContext inside the API
            builder.Services.AddDbContext<TechMoveGLMS.API.Data.ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // --- START OF JWT AUTHENTICATION SERVICES CONFIGURATION ---
            // Dynamically reads security configuration keys out of appsettings.json
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var jwtKey = jwtSettings["Key"];
            var jwtIssuer = jwtSettings["Issuer"];
            var jwtAudience = jwtSettings["Audience"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
            // --- END OF JWT AUTHENTICATION SERVICES CONFIGURATION ---

            // Add services to the container.
            builder.Services.AddControllers();

            // Correct .NET 10 method syntax to set the document name to 'v1'
            builder.Services.AddOpenApi("v1");

            // Enables the API Explorer endpoints required for documentation
            builder.Services.AddEndpointsApiExplorer();

            // 🛠️ Registers Swagger generation tools
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // 🛠️ Enable the interactive Swagger UI interface in development mode
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // This ensures Swagger endpoints map correctly inside Docker
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GLMS API v1");
                });
            }

            // Map the OpenAPI JSON document endpoint
            app.MapOpenApi();

            // This directs the default root URL straight to the OpenAPI document path smoothly
            app.MapGet("/", () => Results.Redirect("/openapi/v1.json"));

            app.UseHttpsRedirection();

            // --- STRATEGIC MIDDLEWARE ORDER FOR SECURITY ---
            app.UseAuthentication(); // 1. Identifies who the user is using the token
            app.UseAuthorization();  // 2. Checks if the identified user has permission

            app.MapControllers();

            app.Run();
        }
    }
}