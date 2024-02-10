using SalesManager.Application;
using SalesManager.Application.Extensions;
using SalesManager.Application.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", true, true);

builder.Host.UseSerilog(builder.ConfigureLogger());

builder.Services.AddControllers().ConfigureJsonOptions();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

await builder.Services.EnsureSuperAdminExists();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerTool();

app.UseAutomaticMigration<DatabaseContext>();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
