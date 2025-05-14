using Amazon.CloudWatchLogs;
using Routes;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;
using YaushServer.Url;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureLogging(builder);

builder.Services.AddScoped<ICreateUrlService, CreateUrlService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.MapGet("/", async (ICreateUrlService createUrlService, ILogger<DefaultRoute> logger) =>
{
    logger.LogInformation("At Minimal Endpoint");
    await createUrlService.CreateUrl("https://google.com");
});

app.Run();


static void ConfigureLogging(WebApplicationBuilder builder)
{
    var client = new AmazonCloudWatchLogsClient();

    builder.Logging.ClearProviders();
    Serilog.ILogger logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.AmazonCloudWatch(
            logGroup: $"{builder.Environment.EnvironmentName}/{builder.Environment.ApplicationName}",
            logStreamPrefix: DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            cloudWatchClient: client
            )
        .CreateLogger();
    builder.Services.AddSerilog(logger);
}

namespace Routes
{
    public class DefaultRoute { }
}