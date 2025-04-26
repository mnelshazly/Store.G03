using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Services.Abstractions;

namespace Services
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);

        private readonly Lazy<IBasketService> _LazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
        public IBasketService BasketService => _LazyBasketService.Value;
    }
}
