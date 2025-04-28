
using Domain.Contracts;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Idenetity;
using Services;
using Services.Abstractions;
using Shared.ErrorModels;
using Store.G03.Api.CustomMiddleWare;
using Store.G03.Api.Extensions;
using Store.G03.Api.Factories;
using AssemblyMapping = Services.AssemblyReference;

namespace Store.G03.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();

            #endregion

            var app = builder.Build();

            #region Seeding

            await app.SeedDataBaseAsync();

            #endregion

            // Configure the HTTP request pipeline.

            #region Configure the HTTP request pipeline.

            app.UseCustomExceptionMiddleWare();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
