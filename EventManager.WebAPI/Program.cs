using EventManager.BLL.Interfaces;
using EventManager.BLL.Services;
using EventManager.DAL.Interfaces;
using EventManager.DAL.Repositories;
using EventManager.WebAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDbConnection>(service =>
{
    string connectionString = builder.Configuration.GetConnectionString("Default");
    return new SqlConnection(connectionString);
});

builder.Services.AddTransient<IMemberRepository, MemberRepository>();
builder.Services.AddTransient<IActivityRepository, ActivityRepository>();
builder.Services.AddTransient<IRegistrationRepository, RegistrationRepository>();

builder.Services.AddTransient<IMemberService, MemberService>();
builder.Services.AddTransient<IActivityService, ActivityService>();

builder.Services.AddTransient<TokenService>();

builder.Services.AddControllers();

// Add JWT config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Secret"]))
        };
    });

// Add Cors config
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("DemoCors", config =>
    {
        // Tout autoriser
        config.AllowAnyOrigin();
        config.AllowAnyHeader();
        config.AllowAnyMethod();

        // Limiter l'origine de la requete
        // config.WithOrigins("http://127.0.0.1:5500");
    });
});

// Config Swagger/OpenAPI (Doc : https://aka.ms/aspnetcore/swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Ajout des informations sur l'API
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Event Manager API",
        Description = "Web API pour créer et rejoindre des evenement",
        License = new OpenApiLicense
        {
            Name = "MIT License",
        }
    });

    // Ajout du bouton d'autentification
    c.AddSecurityDefinition("Auth Token", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Ajouter le JWT necessaire à l'autentification"
    });

    // Ajout du "verrou" sur toutes les routes de l'API pour exploiter le JWT
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Auth Token" }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("DemoCors");

app.MapControllers();

app.Run();
