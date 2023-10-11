using System.Text;
using System.Text.Json.Serialization;
using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Helper;
using BlogAPIDotnet6.Repositories.Implementations;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // informações personalizadas sobre a documentação do Swagger
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "Blog.Net",
        Description = "Projeto destinado a portfólio. Sinta-se à vontade ao utilizá-lo. :)",
        Contact = new OpenApiContact
        {
            Name = "Romulo de Oliveira",
            Email = "dev@romulodeoliveira.net",
            Url = new Uri("https://romulodeoliveira.net/"),
        },
        License = new OpenApiLicense
        {
            Name = "Licença",
            Url = new Uri("https://github.com/romulodeoliveira/BlogAPIDotnet6/blob/main/LICENSE.md"),
        }
    });
    
    // Informações do oauth2
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new TokenHelper().Key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Configuração do banco de dados
string dbConfig = "Server=localhost;Port=3306;Database=blogtechapi;Uid=arch;Pwd=1234;";
builder.Services.AddDbContextPool<DataContext>(options => options.UseMySql(dbConfig, ServerVersion.AutoDetect(dbConfig)));

// Injeção de dependência
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
