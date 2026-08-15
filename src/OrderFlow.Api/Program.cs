using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Inventory.Repositories;
using OrderFlow.Application.Inventory.Services;
using OrderFlow.Application.Products.Repositories;
using OrderFlow.Application.Products.Services;
using OrderFlow.Infrastructure.Inventory;
using OrderFlow.Infrastructure.Persistence;
using OrderFlow.Infrastructure.Products;
using OrderFlow.Api.ExceptionHandling;
using OrderFlow.Application.Customers.Repositories;
using OrderFlow.Application.Customers.Services;
using OrderFlow.Infrastructure.Customers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<OrderFlowDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
