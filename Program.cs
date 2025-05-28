using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Sinks.OpenTelemetry;
using YaushServer.Url;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureLogging(builder);
builder.Services.AddNpgsqlDataSource(
    $"Host={builder.Configuration["YaushDB:Host"]};" +
    $"Username={builder.Configuration["YaushDB:Username"]};" +
    $"Password={builder.Configuration["YaushDB:Password"]};" +
    $"Database=yaush");

builder.Services.AddSingleton<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<ICreateUrlService, CreateUrlService>();
builder.Services.AddScoped<IGetUrlService, GetUrlService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapControllers();

app.RegisterUrlEndpoints();

app.Run();


static void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Logging.ClearProviders();
    Serilog.ILogger logger;

    if (builder.Environment.IsDevelopment())
    {
        logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.OpenTelemetry(opt =>
            {
                opt.IncludedData =
                    IncludedData.SpanIdField
                    | IncludedData.TraceIdField
                    | IncludedData.TemplateBody
                    | IncludedData.MessageTemplateTextAttribute
                    | IncludedData.MessageTemplateMD5HashAttribute;

                opt.ResourceAttributes = new Dictionary<string, object>
                {
                    { "service.name", builder.Environment.ApplicationName }
                };

                opt.Endpoint = "http://seq:5341/ingest/otlp/v1/logs";
                opt.Protocol = OtlpProtocol.HttpProtobuf;
                opt.Headers = new Dictionary<string, string>
                {
                    { "X-Seq-ApiKey", "vlynan9PEvkDza7XIB0p" },
                };
            })
            .CreateLogger();
    }
    else
    {
        var client = new AmazonCloudWatchLogsClient();
        logger = new LoggerConfiguration()
            .WriteTo.AmazonCloudWatch(
                logGroup: $"{builder.Environment.EnvironmentName}/{builder.Environment.ApplicationName}",
                logStreamPrefix: DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                cloudWatchClient: client
            )
        .CreateLogger();
    }

    builder.Services.AddSerilog(logger);
}