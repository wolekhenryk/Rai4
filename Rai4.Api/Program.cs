using System.Reflection;
using Rai4.Api.Configuration;
using Rai4.Application;
using Rai4.Application.WebSockets;
using Rai4.Infrastructure;
using Rai4.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add API services
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    foreach (var xml in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
        c.IncludeXmlComments(xml, includeControllerXmlComments: true);

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Rai4 API",
        Version = "v1",
        Description = "API for Rai4 application",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "h.wolek@maritex.eu"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", doc), []
        }
    });
});
builder.Services.AddHttpContextAccessor();

// Add layer-specific services
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Run database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure middleware pipeline
app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// CORS must be before authentication
app.UseCors("Rai4Policy");

// Authentication & Authorization middleware (order matters!)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map SignalR hub
app.MapHub<BusStopHub>("/hubs/busstop");

app.Run();