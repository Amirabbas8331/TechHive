
using TechHive.Presentation.Extentions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.
builder.AddApplicationBuilder();

var app = builder.Build();

app.UseWebAppMiddleware();
