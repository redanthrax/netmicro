
namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);
public class GetProductByIdEndpoint : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        app.MapGet("/products/{id}", async (int id, ISender sender) => {
            var response = await sender.Send(new GetProductByIdQuery(id));
            return response.Product is not null ? (IResult)Results.Ok(response) : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}
