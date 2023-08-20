using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Repositories.Implementations;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string dbConfig = "Server=localhost;Port=3306;Database=blogtechapi;Uid=arch;Pwd=1234;";
builder.Services.AddDbContextPool<DataContext>(options => options.UseMySql(dbConfig, ServerVersion.AutoDetect(dbConfig)));

builder.Services.AddScoped<IUserRepository, UserRepository>();

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
