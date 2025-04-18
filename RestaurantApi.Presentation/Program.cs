using Microsoft.EntityFrameworkCore;
using RestaurantApi.Application.Services;
using RestaurantApi.Infrastructure.Persistence.Data;
using RestaurantApi.Infrastructure.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<RestaurantDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });

    builder.Services.AddScoped<IDishService, DishService>();
    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddControllers();
}

var app = builder.Build();
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
        dbContext.Database.EnsureCreated(); // Creates DB and tables if not already created
    }

    app.MapControllers();

    app.Run();
}
