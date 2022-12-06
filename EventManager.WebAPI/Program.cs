using EventManager.BLL.Interfaces;
using EventManager.BLL.Services;
using EventManager.DAL.Interfaces;
using EventManager.DAL.Repositories;
using EventManager.WebAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
