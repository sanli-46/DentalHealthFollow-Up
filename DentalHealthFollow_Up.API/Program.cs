using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.API.Options;
using DentalHealthFollow_Up.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 0) Development'ta user-secrets'
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();

}

// 1) Kestrel portlarý
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5050); 
    serverOptions.ListenAnyIP(7250, listenOptions => listenOptions.UseHttps());
});

// 2) DbContext 
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// 3) CORS (MAUI)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("allow-maui", p =>
        p.AllowAnyHeader()
         .AllowAnyMethod()
         .WithOrigins("http://localhost", "https://localhost"));
});

// 4) Options binding + Servisler 
builder.Services.Configure<EncryptionOptions>(
    builder.Configuration.GetSection("Encryption"));
builder.Services.AddSingleton<EncryptionService>();

builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection("Smtp"));

builder.Services.AddScoped<MailService>();

// 5) MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 6) Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("allow-maui");
app.UseAuthorization();
app.MapControllers();

app.Run();
