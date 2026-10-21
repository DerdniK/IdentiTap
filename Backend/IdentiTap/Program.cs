using System.Text;
using IdentTap.API.Endpoints;
using IdentTap.Application.Interfaces;
using IdentTap.Application.Services;
using IdentTap.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddInfrastructure();

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "CLAVE_SECRETA_SUPER_SEGURA_DE_AL_MENOS_32_BYTES_123456";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "IdentTapApi",
            ValidAudience = "IdentTapClients",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapNfcEndpoints();

app.Run();