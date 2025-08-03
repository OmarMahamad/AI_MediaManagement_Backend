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
using Microsoft.OpenApi.Models;
using ServiceLayer.AuthorizationStatus.Interface;
using ServiceLayer.AuthorizationStatus.Service;
using ServiceLayer.SubscriptionService.Interface;
using ServiceLayer.SubscriptionService.Service;
using PaymentLayer.PaypalService.Interface;
using PaymentLayer.PaypalService.Service;
using ServiceLayer.PaymentTransactionService.Interface;
using ServiceLayer.PaymentTransactionService.Service;


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
        ValidAudience = configuration["Jwt:Audience"], // ← Audience واحدة

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


// JWT in Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Back end API", Version = "v1" });

    // تعريف المصادقة بـ JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "ادخل 'Bearer' ثم مسافة ثم التوكن بتاعك.\n\nمثال: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    // ربط المصادقة بجميع العمليات
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



//var secc = builder.Configuration["Jwt:Key"];
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DbApp>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"))); // DB connection with Sql
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryService<>));
builder.Services.AddScoped(typeof(IAuthorization), typeof(AuthorizationService));
builder.Services.AddScoped(typeof(IUsre), typeof(UserService));
builder.Services.AddScoped(typeof(IAuthStatus), typeof(AuthStatusService));
builder.Services.AddScoped(typeof(ISubscription), typeof(SubscriptionService));
builder.Services.AddScoped(typeof(IPayment), typeof(PymentService));

builder.Services.AddHttpClient<IPaypal,PaypalServiceApi>();


builder.Services.AddTransient<Isecurity, SecurityService>();
builder.Services.AddScoped<IFile, FileService>();
builder.Services.AddTransient<IEmail, EmailService>();
builder.Services.AddSingleton<MessageService>();

// key: r2YX/FMEHXD9I2wF3ySuUR21mGU/bIvhBL91ZgUnG06xT/5IohJstojMuM9Vt4aXVFd0VHTc5714OGpK7SC2qw==

// paypal

// Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontAPI",
        policy => policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontAPI");
//app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
