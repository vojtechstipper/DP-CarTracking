using System.Text.Json.Serialization;
using CarTracking.API;
using CarTracking.API.Middlewares;
using CarTracking.BE.Application;
using CarTracking.BE.Application.Invocables;
using CarTracking.BE.Application.Mappings;
using CarTracking.BE.Application.Options;
using Coravel;
using Coravel.Scheduling.Schedule.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Error);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddAndConfigureSwagger();
builder.Services.AddAutoMapper(typeof(LocationProfile));
builder.Services.AddApplication();

builder.Services.AddScheduler();
builder.Services.AddTransient<CheckStatusesInvocable>();
builder.Services.AddTransient<CacheCheckerInvocable>();

builder.Services.AddMemoryCache();
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));


builder.Services.AddRepositories();
builder.Services.AddJwtToken();
builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

app.ApplyMigrations();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        "cartracking-mobileapp-firebase-adminsdk-jw80e-e640878aef.json")),
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseCors("MyPolicy");

app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<CheckStatusesInvocable>()
        .DailyAt(1, 0)
        .PreventOverlapping(nameof(CheckStatusesInvocable));
    scheduler.Schedule<CacheCheckerInvocable>()
        .EveryMinute()
        .PreventOverlapping(nameof(CacheCheckerInvocable));
}).LogScheduledTaskProgress(app.Services.GetRequiredService<ILogger<IScheduler>>());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();