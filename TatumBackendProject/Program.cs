using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TatumBackendProject.Auth;
using TatumBackendProject.Common.Constants;
using TatumBackendProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//EF Core
builder.Services.AddDbContext<TatumBackendProject.Data.AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository pattern - register repositories and UnitOfWork
builder.Services.AddScoped<TatumBackendProject.Repositories.IUserRepository, TatumBackendProject.Repositories.UserRepository>();
builder.Services.AddScoped<TatumBackendProject.Repositories.IAccountRepository, TatumBackendProject.Repositories.AccountRepository>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.ITransactionRepository, TatumBackendProject.Repositories.TransactionRepository>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.ITransferRepository, TatumBackendProject.Repositories.TransferRepository>();
builder.Services.AddScoped<TatumBackendProject.Repositories.INotificationRepository, TatumBackendProject.Repositories.NotificationRepository>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.IRefreshTokenRepository, TatumBackendProject.Repositories.RefreshTokenRepository>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.IUnitOfWork, TatumBackendProject.Repositories.UnitOfWork>();

// DI
builder.Services.AddScoped<TatumBackendProject.Services.IAuthService, TatumBackendProject.Services.AuthService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.ITransactionRepository, TatumBackendProject.Repositories.TransactionRepository>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.ITransferRepository, TatumBackendProject.Repositories.TransferRepository>();
builder.Services.AddScoped<TatumBackendProject.Repositories.INotificationRepository, TatumBackendProject.Repositories.NotificationRepository>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.IRefreshTokenRepository, TatumBackendProject.Repositories.RefreshTokenRepository>();
//builder.Services.AddScoped<TatumBackendProject.Repositories.IUnitOfWork, TatumBackendProject.Repositories.UnitOfWork>();


//JWT Token helper
builder.Services.AddScoped<TatumBackendProject.Auth.JwtService>();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddScoped<
    TatumBackendProject.Services.IEmailSender,
    SmtpEmailSender>();

builder.Services.AddHttpClient();
builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("Sms"));
builder.Services.AddHttpClient<INotificationService, NotificationService>();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "TatumConnect API",
            Version = "v1",
            Description = "TatumConnect Digital Banking API"
        }
    );

    // ==========================
    // JWT BEARER AUTHENTICATION
    // ==========================

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT access token."
        }
    );

    // ==========================================
    // APPLY JWT ATHENTICATION TO SWAGGER
    // ============================================

    options.AddSecurityRequirement(
        document =>
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(
                    "Bearer",
                    document)] = []
            }
    );
});

var jwtSettings =
    builder.Configuration
        .GetSection("JwtSettings")
        .Get<JwtSettings>();

if (jwtSettings == null)
{
    throw new InvalidOperationException(
        "JwtSettings are not configured.");
}

if (string.IsNullOrWhiteSpace(
    jwtSettings.Secret))
{
    throw new InvalidOperationException(
        "JWT Secret is not configured.");
}

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtSettings.Secret)),
                RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
            };
    });

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "TatmConnect API v1"
    );

    options.RoutePrefix = "swagger";
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
