using System.Text.Json;
using FileUploader.API.Extensions;
using FileUploader.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InitializeSwagger(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServicesAndConfigurations(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation(app.Configuration);
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseRouting();
app.ConfigureGlobalHandler(app.Logger);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("health");

app.Run();