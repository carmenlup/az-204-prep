using DataModel.Entities;

namespace DataModel
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var products = new List<Product>
            {
                new()
                {
                    Name = "Mobile",
                    Quantity = 100
                },
                new()
                {
                    Name = "Laptop",
                    Quantity = 200
                },
                new()
                {
                    Name = "Laptop",
                    Quantity = 300
                },
            };

            foreach (var product in products)
            {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}
