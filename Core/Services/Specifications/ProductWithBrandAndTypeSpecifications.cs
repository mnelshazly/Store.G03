﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
    {
        // Get All Products With Types and Brands
        public ProductWithBrandAndTypeSpecifications(ProductQueryParams queryParams) 
            : base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId) && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            switch(queryParams.SortingOption)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                default:
                    break;
            }
        }

        // Get Product By Id
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id) 
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
