using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.Data;
using MinimalAPIDemo.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContextDb(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/employees", async (
    MinimalContextDb context) =>
    await context.Employees.ToListAsync())
    .WithName("GetEmployees")
    .WithTags("Employee");

app.Run();