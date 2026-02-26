using BlogAPI.Data;
using BlogAPI.Repository;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/postLog.txt",rollingInterval:RollingInterval.Month).CreateLogger(); 

//builder.Host.UseSerilog();
builder.Services.AddControllers();

// Set up logging to use console
builder.Logging.ClearProviders(); // Clear default loggers
builder.Logging.AddConsole(); // Add Console logger
builder.Logging.AddDebug();  // Optional: Add Debug logger (for local debugging)

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddHealthChecks();
//var connectionString = Environment.GetEnvironmentVariable("PrivateConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(connectionString));
//builder.Services.AddDbContext<ApplicationDbContext>(option =>
//{
//    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
//    //option.UseNpgsql(Environment.GetEnvironmentVariable("PrivateConnection") ?? builder.Configuration.GetValue<string>("DefaultSQLConnection"));
//});


builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    var connectionString = Environment.GetEnvironmentVariable("PrivateConnection")
                           ?? builder.Configuration.GetConnectionString("DefaultSQLConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string is not set. Ensure 'PrivateConnection' (Railway) or 'DefaultSQLConnection' (local appsettings) is configured.");
    }

    option.UseNpgsql(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "https://reactfrontend-production-5d75.up.railway.app",
                "https://kylesisland.com",
                "https://www.kylesisland.com",
                "http://localhost:5173/"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});





builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers();
builder.Services.AddResponseCaching();

//var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
var key = Environment.GetEnvironmentVariable("Secret") ??
    builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }); ;

builder.Services.AddControllers(option => {
    // option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description =
        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
        "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
        "Example: \"Bearer 12345asdbce\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseHealthChecks("/health");
// Configure the HTTP request pipeline.

    app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowLocalhost");
app.MapControllers();
app.UseCors("AllowFrontend");


app.Run();
