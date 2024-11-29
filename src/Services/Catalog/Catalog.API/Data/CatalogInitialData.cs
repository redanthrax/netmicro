using Marten.Schema;

namespace Catalog.API.Data;
public class CatalogInitialData : IInitialData {
    public async Task Populate(IDocumentStore store, CancellationToken cancellation) {
        using var session = store.LightweightSession();
        if(await session.Query<Product>().AnyAsync()) return;

        //upsert will not overwrite existing documents
        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>() {
        new Product() { Name = "Keyboard", Description = "Wireless keyboard", Price = 20, ImageFile = "product-1.png", Category = ["Electronics"] },
        new Product() { Name = "Mouse", Description = "Wireless mouse", Price = 10, ImageFile = "product-2.png", Category = ["Electronics"] },
        new Product() { Name = "Monitor", Description = "27-inch monitor", Price = 200, ImageFile = "product-3.png", Category = ["Electronics"] },
        new Product() { Name = "Desk", Description = "Wooden desk", Price = 100, ImageFile = "product-4.png", Category = ["Furniture"] },
        new Product() { Name = "Lamp", Description = "Desk lamp", Price = 5, ImageFile = "product-5.png", Category = ["Electronics"] },
        new Product() { Name = "Chair", Description = "Leather chair", Price = 150, ImageFile = "product-6.png", Category = ["Furniture"] },
    };
}
