using Serilog;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var connectionString = builder.Configuration.GetConnectionString("HotelListingDBConnectionString");


builder.Services.AddDbContext<HotelListingDbContext>( o =>
{
    o.UseSqlServer(connectionString);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors( o=>
{
    o.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
    });

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
