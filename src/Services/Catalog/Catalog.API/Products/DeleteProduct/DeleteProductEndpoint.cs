﻿
namespace Catalog.API.Products.DeleteProduct;
partial record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        app.MapDelete("/products/{id}", async (int id, ISender sender) => {
            var result = await sender.Send(new DeleteProductCommand(id));
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete a product")
            .WithDescription("Delete a product by its identifier.");
    }
}
