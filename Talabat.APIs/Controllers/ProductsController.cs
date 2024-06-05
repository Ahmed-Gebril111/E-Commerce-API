using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
   
    public class ProductsController : ApiBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo ,IGenericRepository<ProductType> TypeRepo,IGenericRepository<ProductBrand> BrandRepo ,IMapper mapper )
        {
            _productRepo = productRepo;
            _typeRepo = TypeRepo;
            _brandRepo = BrandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var spec = new ProductWithBrandAndTypeSpecification(Params);
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            //var ReturnedObject = new Pagination<ProductToReturnDto>()
            //{
            //    PageIndex = Params.PageIndex,
            //    PageSize = Params.PageSize,
            //    Data = MappedProducts
            //};
            var CountSpec = new ProductWithFilterationForCountAsync(Params);
            var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex,Params.PageSize,MappedProducts,Count));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var Product = await _productRepo.GetByIdWithSpecAsync(spec);
            if (Product is null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product,ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetType()
        {
            var Types = await _typeRepo.GetAllAsync();
            return Ok(Types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands() 
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }

    }
}
