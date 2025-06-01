using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProjetoGerenciamento.Application.Interfaces;
using ProjetoGerenciamento.Application.Services;
using ProjetoGerenciamento.Domain.Interfaces;
using ProjetoGerenciamento.Infrastructure.Data;
using ProjetoGerenciamento.Infrastructure.Repositories;
using ProjetoGerenciamento.API.Settings;
using ProjetoGerenciamento.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// Add the following using for Swagger
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext com PostgreSQL
builder.Services.AddDbContext<ProjetoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Defensive null check for jwtSettings
    if (jwtSettings is null)
        throw new InvalidOperationException("JWT settings are not configured properly.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ValidateIssuerSigningKey = true
    };
});

// Registrar reposit�rios e servi�os
builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
builder.Services.AddScoped<IProjetoService, ProjetoService>();

// Registrar AutoMapper (disambiguate overload)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registrar Controllers
builder.Services.AddControllers();

// Configurar Swagger com suporte a JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Projeto Gerenciamento API",
        Version = "v1",
        Description = "API para gerenciamento de projetos com autentica��o JWT"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT desta forma: Bearer {token}"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    };

    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

// Middleware global de tratamento de exce��es
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projeto Gerenciamento API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz da aplica��o
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
