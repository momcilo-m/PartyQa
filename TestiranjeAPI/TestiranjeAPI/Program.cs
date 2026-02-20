using AutoMapper;
using Backend.Application.Mappings;
using Microsoft.EntityFrameworkCore;
using TestiranjeAPI.Repository;
using TestiranjeAPI.Models;
using TestiranjeAPI.Repository.Interfaces;
using TestiranjeAPI.Services.Interfaces;
using TestiranjeAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PartyContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("BazaCS"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", builder =>
    {
        builder.WithOrigins(new string[] { "http://127.0.0.1:5500" })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPartyRepository, PartyRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPartyService, PartyService>();  
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CORS");

app.UseAuthorization();

app.MapControllers();

app.Run();
