using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskHive.API.Data;
using TaskHive.API.Models;
using TaskHive.API.Providers;
using TaskHive.API.Repositories;
using TaskHive.API.Services;
using TaskHive.API.Settings;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration
    .Get<AppSettings>();

var jwtSettings = builder.Configuration
    .GetSection(nameof(AppSettings.JWTSettings))
    .Get<JWTSettings>();

var connectionStrings = builder.Configuration
    .GetSection(nameof(AppSettings.ConnectionStrings))
    .Get<ConnectionStrings>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(appSettings!.ConnectionStrings.Application);
});

builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration["JWTSettings:Secret"];
        var audience = builder.Configuration["JWTSettings:Audience"];
        var issuer = builder.Configuration["JWTSettings:Issuer"];

        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JWT secret is missing");
        }

        if (string.IsNullOrEmpty(audience))
        {
            throw new InvalidOperationException("JWT audience is missing");
        }

        if (string.IsNullOrEmpty(issuer))
        {
            throw new InvalidOperationException("JWT issuer is missing");
        }

        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = issuerSigningKey,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(appSettings!);
builder.Services.AddSingleton(jwtSettings!);
builder.Services.AddSingleton(connectionStrings!);

builder.Services.AddScoped<ITaskItemService, TaskItemService>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserManagerProvider, UserManagerProvider>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();