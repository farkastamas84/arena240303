using ArenaSimulator.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.Configure();

await app.InitializeAsync();

app.Run();