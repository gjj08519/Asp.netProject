using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PizzaWebsite.Data.Seeder
{
    public class PizzaWebsiteDataSeeder
    {
        private readonly IWebHostEnvironment _host;
        private readonly UserIdentityDbContext _identityContext;
        private readonly PizzaWebsiteDbContext _context;

        public PizzaWebsiteDataSeeder(IWebHostEnvironment host, UserIdentityDbContext identityContext, PizzaWebsiteDbContext context)
        {
            _host = host;
            _identityContext = identityContext;
            _context = context;
        }

        public void Seed()
        {            
            // ensure that the database exists
            _context.Database.EnsureCreated();

            // if there are no user datas
            if (!_context.UserDatas.Any())
            {
                var userdatasFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/userdatas.json");

                var userdatasJson = File.ReadAllText(userdatasFile);

                var userdatas = JsonConvert.DeserializeObject<IEnumerable<UserData>>(userdatasJson).ToList();

                _context.UserDatas.AddRange(userdatas);

                var users = _identityContext.Users.ToList();

                if (users.Count < userdatas.Count)
                {
                    throw new InvalidOperationException("Could not create user datas due to missing users.");
                }

                // attach user ids to user datas
                foreach (UserData userData in userdatas)
                {
                    userData.UserId = users.First(u => u.Email.Split("@")[0].ToLower() == userData.LastName.ToLower()).Id;
                }
            }

            // if there are no Products
            if (!_context.Products.Any())
            {
                // create sample product data

                // ContentRootPath is refering to folders not related to wwwroot
                var productsFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/products.json");
                var json = File.ReadAllText(productsFile);

                // deserialize json file into a list of deserializedProducts
                var deserializedProducts = JsonConvert.DeserializeObject<IEnumerable<DeserializedProduct>>(json);

                List<Portion> portions = new List<Portion>();

                foreach (DeserializedProduct deserializedProduct in deserializedProducts)
                {
                    Product product = new Product()
                    {
                        Name = deserializedProduct.Name,
                        Description = deserializedProduct.Description,
                        ImageName = deserializedProduct.ImageName
                    };

                    // Add Category
                    product.Category = deserializedProduct.Category.ToUpper() switch
                    {
                        "PIZZA" => ProductCategory.Pizza,
                        "BURGER" => ProductCategory.Burger,
                        "DRINK" => ProductCategory.Drink,
                        _ => ProductCategory.Side,
                    };

                    // Add all Portions + Prices
                    for (int portionPriceIterator = 0; portionPriceIterator < deserializedProduct.Portions.Count; portionPriceIterator++)
                    {
                        // Get or create a new Portion, depending on if it exists or not
                        Portion portion = portions.Where(p => p.Label.ToUpper() == deserializedProduct.Portions[portionPriceIterator].ToUpper()).FirstOrDefault();
                        if (portion == null)
                        {
                            portion = new Portion()
                            {
                                Label = deserializedProduct.Portions[portionPriceIterator]
                            };
                            portions.Add(portion);
                        }

                        // Set up the ProductPortion that connects the Product and Portion
                        ProductPortion productPortion = new ProductPortion()
                        {
                            Product = product,
                            Portion = portion,
                            UnitPrice = deserializedProduct.Prices[portionPriceIterator]
                        };

                        // Finalize the relationship between the Product and Portion
                        _context.ProductPortions.Add(productPortion);
                    }

                    // Add the product now that it is complete
                    _context.Products.Add(product);
                }

                // Add the portions now that they are complete
                _context.Portions.AddRange(portions);
            }

            
            if (!_context.Carts.Any())
            {
                SeedCart();
            }

            // Commit changes to the database
            _context.SaveChanges();
        }

        private void SeedCart()
        {

        }
    }
}
