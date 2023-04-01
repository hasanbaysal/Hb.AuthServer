using Hb.AuthServer.Common.Exceptions;
using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Entity;
using Hb.AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hb.AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : CustomBaseController
    {

        private readonly IGenericService<Product, ProductDto> productService;

        public ProductController(IGenericService<Product, ProductDto> productService)
        {
            this.productService = productService;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return ActionResultInstance(await productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductDto dto)
        {

            return ActionResultInstance(await productService.AddAsync(dto));
        }
        [HttpPut]
        public async Task<IActionResult> Update(ProductDto dto)
        {

            return ActionResultInstance(await productService.Update(dto, dto.Id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {

            return ActionResultInstance(await productService.Remove(id));
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> CustomExceptionTest()
        {

            throw new CustomException("database exception.....");

            
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> CustomExceptionTest2()
        {

            throw new DivideByZeroException();


        }

    }
}
