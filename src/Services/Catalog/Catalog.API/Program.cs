using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter(configurator: c => {
    var modules = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => typeof(ICarterModule).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
        .ToArray();
    c.WithModules(modules);
});

builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment()) {
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// configure the http pipeline
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", 
    new HealthCheckOptions {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    });
app.Run();