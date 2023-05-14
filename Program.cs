using Serilog;
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, loggerConfig) => loggerConfig.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddControllers(options => {
    // add global filters
    options.Filters.Add<LoggingFilter>();
    options.Filters.Add<AsyncLoggingFilter>();
});

// register this filter to use via ServceFilter attribute to allow it to use DI
builder.Services.AddScoped<LoggingExceptionAttribute>();

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

app.MapControllers();

app.Run();
