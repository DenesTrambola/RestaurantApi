using Microsoft.EntityFrameworkCore;
using RestaurantApi.Application.Services;
using RestaurantApi.Infrastructure.Persistence.Data;
using RestaurantApi.Infrastructure.Persistence.Services;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<RestaurantDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateLogger();
    builder.Host.UseSerilog();

    builder.Services.AddScoped<IDishService, DishService>();
    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
}

var app = builder.Build();
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
        dbContext.Database.EnsureCreated();
    }

    if (app.Environment.EnvironmentName.Equals("Development"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
