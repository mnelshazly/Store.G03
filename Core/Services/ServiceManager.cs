using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Abstractions;

namespace Services
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);

        private readonly Lazy<IBasketService> _LazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
        public IBasketService BasketService => _LazyBasketService.Value;
        private readonly Lazy<IAuthenticationService> _LazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, configuration, mapper));

        public IAuthenticationService AuthenticationService => _LazyAuthenticationService.Value;

    }
}
