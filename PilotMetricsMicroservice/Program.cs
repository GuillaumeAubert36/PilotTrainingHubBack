// Program.cs → Controller → Service → Repository → DB

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddScoped<IMetricsRepository, MetricsRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

var logger = app.Logger;
logger.LogInformation("PilotMetricsMicroservice application starting...");

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Metrics API v1");
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();