using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {

        public static async Task SeedAsync(StoreContext dbcontext)
        {

            //Seeding Brands

            if (!dbcontext.Brands.Any())
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbcontext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }


            //Seeding Types

            if (!dbcontext.Types.Any())
            {
                var TypeData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                    {
                        await dbcontext.Set<ProductType>().AddAsync(Type);
                    }
                    await dbcontext.SaveChangesAsync();
                }

            }


            //Seeding Products

            if (!dbcontext.Products.Any())
            {
                var ProdctData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProdctData);
                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                    {
                        await dbcontext.Set<Product>().AddAsync(Product);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }


            //Seeding Deliveries

            if (!dbcontext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (deliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in deliveryMethods)
                    {
                        await dbcontext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }





        }



    }
}
