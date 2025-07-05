using Microsoft.EntityFrameworkCore;
using RepositoryLayer;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthorizationLayer.Interface;
using AuthorizationLayer.Service;
using SecurityLayer.Intercafe;
using SecurityLayer.Service;
using HelperLayer.Notifecation.Email.Interface;

using HelperLayer.Constants.Services;
using ServiceLayer.UserStatus.Interface;
using ServiceLayer.UserStatus.Service;
using HelperLayer.File.Service;
using HelperLayer.File.Interface;
using HelperLayer.Notifecation.Email.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudiences = configuration.GetSection("Jwt:Audiences").Get<string[]>(),

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };


});
// convert enum to string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//var secc = builder.Configuration["Jwt:Key"];
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DbApp>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"))); // DB connection with Sql
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryService<>));
builder.Services.AddScoped(typeof(IAuthorization), typeof(AuthorizationService));
builder.Services.AddScoped(typeof(IUsre), typeof(UserService));
builder.Services.AddTransient<Isecurity, SecurityService>();
builder.Services.AddScoped<IFile, FileService>();
builder.Services.AddTransient<IEmail, EmailService>();
builder.Services.AddSingleton<MessageService>();

// key: r2YX/FMEHXD9I2wF3ySuUR21mGU/bIvhBL91ZgUnG06xT/5IohJstojMuM9Vt4aXVFd0VHTc5714OGpK7SC2qw==

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

app.MapControllers();

app.Run();
