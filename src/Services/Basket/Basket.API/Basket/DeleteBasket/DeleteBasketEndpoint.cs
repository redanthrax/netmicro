﻿
namespace Basket.API.Basket.DeleteBasket; 

public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndpoint : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) => {
            var result = await sender.Send(new DeleteBasketCommand(userName));
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Deletes a basket")
            .WithDescription("Deletes a basket for a user");
    }
}