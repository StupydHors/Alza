using Application;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/products", async (IProductRepository productRepository) =>
    {
        var all = await productRepository.GetAll();

        return all;
    })
    .WithName("GetAllProducts")
    .WithOpenApi();

app.MapGroup("/v2").MapGet("/products", async (IProductRepository productRepository, [FromQuery] int pageNumber, [FromQuery] int? pageSize) =>
    {
        var pagedProducts = await productRepository.GetPaged(pageNumber, pageSize ?? 10);

        return pagedProducts;
    })
    .WithName("GetAllProductsV2")
    .WithOpenApi();

app.MapPost("/products", async ([FromBody] Product product, IProductRepository productRepository) =>
    {
        return await productRepository.Add(product);
    })
    .WithName("InsertProduct")
    .WithOpenApi();

app.MapGet("/products/{id}", async ([FromQuery] Guid id, IProductRepository productRepository) =>
    {
        var product = await productRepository.GetById(id);

        return product;
    })
    .WithName("GetProductById")
    .WithOpenApi();

app.MapDelete("/products/{id}", async ([FromQuery] Guid id, IProductRepository productRepository) =>
    {
        await productRepository.DeleteById(id);
    })
    .WithName("DeleteProduct")
    .WithOpenApi();

app.MapPut("/products/{id}", async ([FromQuery] Guid id, [FromBody] string? description, IProductRepository productRepository) =>
    {
        await productRepository.UpdateDescription(id, description);
    })
    .WithName("UpdateProduct")
    .WithOpenApi();

app.Run();