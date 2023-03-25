using API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();
app.AddMiddlewares();
app.Run();