using System.Text.Json.Serialization;
using NotificationService.ApplicationCore.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding RateLimit Settings from AppSettings
var rateLimitConfig = new RateLimitConfig();
builder.Configuration.GetSection("RateLimitConfig").Bind(rateLimitConfig);
builder.Services.AddSingleton(rateLimitConfig);

builder.Services.AddAppCoreSettings();

//Adding Controller Services
builder.Services.AddControllers(_ =>
    {
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
            
builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy", policyBuilder => 
        policyBuilder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
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