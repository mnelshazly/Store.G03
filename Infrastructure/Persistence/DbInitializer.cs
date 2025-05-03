using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Domain.Contracts;
using Domain.Models.IdentityModule;
using Domain.Models.OrderModule;
using Domain.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Idenetity;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly StoreIdentityDbContext _identityDbContext;

        public DbInitializer(StoreDbContext context,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            StoreIdentityDbContext identityDbContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _identityDbContext = identityDbContext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Create Database if it does not exist && Apply any pending migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }

                // Data Seeding

                // Seeding for ProductTypes from JSon file

                if (!_context.ProductTypes.Any())
                {
                    // 1. Read All Data from types JSon file as String
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    // 2. Transform String to C# Object List<ProductTypes>
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    // 3. Add List<ProductTypes> to Database
                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }

                // Seeding for ProductBrands from JSon file

                if (!_context.ProductBrands.Any())
                {
                    // 1. Read All Data from brands JSon file as String
                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    // 2. Transform String to C# Object List<Productbrands>
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    // 3. Add List<Productbrands> to Database
                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }
                }

                // Seeding for Products from JSon file

                if (!_context.Products.Any())
                {
                    // 1. Read All Data from products JSon file as String
                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    // 2. Transform String to C# Object List<Products>
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    // 3. Add List<Products> to Database
                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }
                }

                // Seeding for Delivery Methods from JSon file

                if (!_context.Set<DeliveryMethod>().Any())
                {
                    // 1. Read All Data from products JSon file as String
                    var deliveryMethodData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");

                    // 2. Transform String to C# Object List<Products>
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

                    // 3. Add List<Products> to Database
                    if (deliveryMethods is not null && deliveryMethods.Any())
                    {
                        await _context.Set<DeliveryMethod>().AddRangeAsync(deliveryMethods);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Elshazly",
                        PhoneNumber = "0123456789",
                        UserName = "mnelshazly"
                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "Rawan@gmail.com",
                        DisplayName = "Rawan Mohamed",
                        PhoneNumber = "0123456789",
                        UserName = "RawanMohamed"
                    };

                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }

                await _identityDbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
