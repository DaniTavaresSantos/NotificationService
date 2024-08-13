using System.Text.Json.Serialization;
using NotificationService.Application.Settings;
using NotificationService.Infra.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding RateLimit Settings from AppSettings
var rateLimitConfig = new RateLimitConfig();
builder.Configuration.GetSection("RateLimitConfig").Bind(rateLimitConfig);
builder.Services.AddSingleton(rateLimitConfig);

//Adding Redis Service
var cacheConnectionString = builder.Configuration.GetConnectionString("Cache");
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = cacheConnectionString);
builder.Services.AddAppCoreSettings();
builder.Services.AddInfraSettings();


//Adding Controller Services
builder.Services.AddControllers(_ =>
    {
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();