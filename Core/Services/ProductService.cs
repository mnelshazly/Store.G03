using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            // Get All Products Through ProductRepository
            var specifications = new ProductWithBrandAndTypeSpecifications();
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specifications);

            // Mapping IEnumerable<Product> to IEnumerable<ProductResultDto> : AutoMapper
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return result;
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(specifications);
            //var product = await unitOfWork.GetRepository<Product, int>().GetAsync(id);
            if (product is null) return null;

            var result = mapper.Map<ProductResultDto>(product);
            
            return result;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }

    }
}
