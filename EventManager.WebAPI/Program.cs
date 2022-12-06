using EventManager.BLL.Interfaces;
using EventManager.BLL.Services;
using EventManager.DAL.Interfaces;
using EventManager.DAL.Repositories;
using EventManager.WebAPI.Helpers;
using System.Data;
using System.Data.SqlClient;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
