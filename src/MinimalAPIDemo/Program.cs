using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.Data;
using MinimalAPIDemo.Extensions;
using MinimalAPIDemo.Models;
using MiniValidation;

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

app.MapGet("employees/{id}", async (
    Guid id,
    MinimalContextDb context) =>
    await context.Employees.FindAsync(id)
    is Employee employee ? Results.Ok(employee) : Results.NotFound())
    .Produces<Employee>(StatusCodes.Status200OK)
    .Produces<Employee>(StatusCodes.Status404NotFound)
    .WithName("GetEmployeeById")
    .WithTags("Employee");

app.MapPost("/employees", async (
    MinimalContextDb context,
    Employee employee) =>
    {
        if (!MiniValidator.TryValidate(employee, out var erros))
            return Results.ValidationProblem(erros);

        context.Employees.Add(employee);
        var employeeCallback = await context.SaveChangesAsync();

        return employeeCallback > 0
            ? Results.CreatedAtRoute("GetEmployeeById", new { id = employee.Id }, employee)
            : Results.BadRequest("Error on saving the register.");
    })
    .ProducesValidationProblem()
    .Produces<Employee>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PostEmployee")
    .WithTags("Employee");

app.Run();