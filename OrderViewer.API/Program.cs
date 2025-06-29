using Microsoft.EntityFrameworkCore;
using OrderViewer.API.Data;
using OrderViewer.API.Repositories;
using OrderViewer.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrderViewerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderViewerConnectionStr")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Define CORS policy
var blazorUrls = builder.Configuration.GetSection("Cors:BlazorApp").Get<string[]>() ?? new string[] { };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfiguredOrigins", policy =>
    {
        policy.WithOrigins(blazorUrls!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto migrate + seed
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderViewerDbContext>();
    context.Database.Migrate();
    DbSeeder.Seed(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowConfiguredOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
