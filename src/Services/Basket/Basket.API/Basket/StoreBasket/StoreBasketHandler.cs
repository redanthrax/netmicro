namespace Basket.API.Basket.StoreBasket;

public record struct StoreBasketCommand(string UserName, ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record struct StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand> {
    public StoreBasketCommandValidator() {
        RuleFor(x => x.Cart).NotNull();
        RuleFor(x => x.Cart.UserName).NotEmpty();
    }
}
public class StoreBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult> {
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken) {
        await repository.StoreBasket(command.Cart, cancellationToken);
        return new StoreBasketResult(command.Cart.UserName);
    }
}
