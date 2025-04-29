using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DataTransferObjects.BasketModuleDtos;

namespace Presentation.Controllers
{
    public class BasketController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Get Basket
        [HttpGet] // GET: baseUrl/api/Basket
        public async Task<ActionResult<BasketDto>> GetBasket(string key)
        {
            var Basket = await _serviceManager.BasketService.GetBasketAsync(key);
            return Ok(Basket);
        }

        // Create or Update Basket
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
        {
            var Basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }

        // Delete Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string key)
        {
            var Basket = await _serviceManager.BasketService.DeleteBasketAsync(key);
            return Ok(Basket);
        }
    }
}
