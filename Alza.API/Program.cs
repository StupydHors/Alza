using System.Reflection;
using Application;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/products", async (IProductRepository productRepository) =>
    {
        var all = await productRepository.GetAll();
        return all;
    })
    .WithName("GetAllProducts")
    .WithOpenApi();

app.MapPost("/products", async ([FromBody] Product product, IProductRepository productRepository) =>
    {
        await productRepository.Add(product);
    })
    .WithName("InsertProduct")
    .WithOpenApi();

app.Run();