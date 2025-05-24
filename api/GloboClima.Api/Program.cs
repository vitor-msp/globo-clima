using GloboClima.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.BuildProject(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.SetupSwagger();
builder.Services.AddHttpLogging(logging => logging.CombineLogs = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseHttpLogging();
app.UseExceptionMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }