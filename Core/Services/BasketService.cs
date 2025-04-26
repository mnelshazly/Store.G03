using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.BasketModule;
using Services.Abstractions;
using Shared.DataTransferObjects.BasketModuleDtos;

namespace Services
{
    public class BasketService (IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> GetBasketAsync(string key)
        {
            var Basket = await _basketRepository.GetBasketAsync(key);
            if (Basket is not null)
                return _mapper.Map<CustomerBasket, BasketDto>(Basket);
            else
                throw new BasketNotFoundException(key);
        }

        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var customerBasket = _mapper.Map<BasketDto, CustomerBasket>(basket);
            var isCreatedOrUpdatedBasket = _basketRepository.CreateOrUpdateBasketAsync(customerBasket);

            if (isCreatedOrUpdatedBasket != null)
                return await GetBasketAsync(basket.Id);
            else
                throw new Exception("Cannot Create or Update the Baasket Now, Try Again Later");
        }

        public async Task<bool> DeleteBasketAsync(string key)
        {
            return await _basketRepository.DeleteBasketAsync(key);
        }
    }
}
