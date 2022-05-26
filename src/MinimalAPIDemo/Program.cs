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
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.CreatedAtRoute("GetEmployeeById", new { id = employee.Id }, employee)
            : Results.BadRequest("Error on saving the register.");
    })
    .ProducesValidationProblem()
    .Produces<Employee>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PostEmployee")
    .WithTags("Employee");

app.MapPut("/employees/{id}", async (
    Guid id,
    MinimalContextDb context,
    Employee employee) =>
    {
        var findEmployee = await context.Employees.FindAsync(id);
        if (findEmployee == null)
            return Results.NotFound();

        if (!MiniValidator.TryValidate(employee, out var errors))
            return Results.ValidationProblem(errors);

        context.Employees.Update(employee);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Error on updating the register.");
    })
    .ProducesValidationProblem()
    .Produces<Employee>(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("PutEmployee")
    .WithTags("Employee");

app.MapDelete("/employees/{id}", async (
    Guid id,
    MinimalContextDb context) => 
    {
        var findEmployee = await context.Employees.FindAsync(id);
        if (findEmployee == null)
            return Results.NotFound();

        context.Employees.Remove(findEmployee);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Error on deleting the register.");
    })
    .ProducesValidationProblem()
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status204NoContent)
    .Produces<Employee>(StatusCodes.Status404NotFound)
    .WithName("DeleteEmployee")
    .WithTags("Employee");

app.Run();