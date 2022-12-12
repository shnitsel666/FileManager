using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FilesManager;
using FilesManager.Extensions;
using FilesManager.Hubs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAppConfig();
builder.Services.AddFilesServices();
builder.Services.AddSignalRService();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1000);
});
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
});
Logger.InitLogger();
builder.WebHost.UseUrls(builder.Configuration["LaunchURl"]);
WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");
app.MapControllers();
app.UseHttpsRedirection();
app.MapHub<FileAgentHub>("/events");
app.Run();
