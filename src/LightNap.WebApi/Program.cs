using LightNap.Core.AnexoFinanceiro.Interface;
using LightNap.Core.Configuration;
using LightNap.Core.Data;
using LightNap.Core.Financas_.Interfaces;
using LightNap.Core.Financas_.Services;
using LightNap.WebApi.Configuration;
using LightNap.WebApi.Extensions;
using LightNap.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.Configure<Dictionary<string, List<SeededUserConfiguration>>>(builder.Configuration.GetSection("SeededUsers"));
builder.Services.AddScoped<IFinancasService, FinancasService>();
builder.Services.AddScoped<IAnexosFinanceiroService, LightNap.Core.AnexoFinanceiro.Service.AnexosFinanceiroService>();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDatabaseServices(builder.Configuration)
    .AddEmailServices(builder.Configuration)
    .AddApplicationServices()
    .AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Middleware pipeline

// Ative swagger tanto no Desenvolvimento quanto em Produção (se quiser, pode condicionar depois)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LightNap API V1");
    c.RoutePrefix = "swagger"; // URL: /swagger/index.html
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseCors(policy =>
    policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Servir arquivos estáticos da pasta "Uploads" para download dos anexos
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

// Servir Angular app na pasta "wwwroot/browser"
string wwwRootPath = app.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
string angularAppPath = Path.Combine(wwwRootPath, "browser");
if (Directory.Exists(angularAppPath))
{
    var fileProvider = new PhysicalFileProvider(angularAppPath);
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        DefaultFileNames = new[] { "index.html" },
        FileProvider = fileProvider
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = fileProvider
    });
    app.MapFallbackToFile("index.html", new StaticFileOptions
    {
        FileProvider = fileProvider,
        RequestPath = ""
    });
}

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var logger = services.GetService<ILogger<Program>>() ?? throw new Exception("Logging is not configured.");

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var applicationSettings = services.GetRequiredService<IOptions<ApplicationSettings>>();

    if (applicationSettings.Value.AutomaticallyApplyEfMigrations && context.Database.IsRelational())
    {
        await context.Database.MigrateAsync();
    }

    Seeder seeder = new(services);
    await seeder.SeedAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration and/or seeding");
    throw;
}

app.Run();
