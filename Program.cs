using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


var app = builder.Build();

app.MapGet("/", () => "API Vendas funcionando!");
app.MapSellerEndpoints();
app.MapTransactionEndpoints();
app.Run();
