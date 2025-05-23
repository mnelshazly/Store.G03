﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Domain.Models.ProductModule;
using Shared.DataTransferObjects.ProductModuleDtos;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductResultDto, string>
    {
        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty; // ""
            else
            {
                //var Url = $"https://localhost:7086/{source.PictureUrl}";
                var Url = $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
                return Url;
            }

        }
    }
}
