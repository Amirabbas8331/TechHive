
using TechHive.Presentation.Extentions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.AddApplicationBuilder();

var app = builder.Build();

app.UseWebAppMiddleware();
