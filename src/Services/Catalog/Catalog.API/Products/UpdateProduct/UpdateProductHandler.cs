
using Catalog.API.Products.CreateProduct;
using FluentValidation;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal Price,
    List<string> Category,
    string ImageFile) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand> {
    public UpdateProductCommandValidator() {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ImageFile).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
internal class UpdateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult> {
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken) {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null) {
            throw new ProductNotFoundException(command.Id);
        }

        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Category = command.Category;
        product.ImageFile = command.ImageFile;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }
}
