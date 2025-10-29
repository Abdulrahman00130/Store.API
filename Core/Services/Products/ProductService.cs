using AutoMapper;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Products;
using Store.API.Domain.Exceptions;
using Store.API.Services.Abstractions.Products;
using Store.API.Services.Specifications;
using Store.API.Services.Specifications.Products;
using Store.API.Shared;
using Store.API.Shared.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Products
{
    public class ProductService (IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters parameters)
        {
            //var spec = new BaseSpecifications<int, Product>(null);
            //spec.Includes.Add(p => p.Brand);
            //spec.Includes.Add(p => p.Type);


            var countSpec = new ProductsCountSpecifications(parameters);
            var count = await _unitOfWork.GetRepository<int, Product>().CountAsync(countSpec);

            var spec = new ProductWithBrandAndTypeSpecifications(parameters);
            var products = await _unitOfWork.GetRepository<int, Product>().GetAllAsync(spec);
            var productsResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);

            var paginationResponse = new PaginationResponse<ProductResponse>(parameters.PageIndex, parameters.PageSize, count, productsResponse);
            return paginationResponse;
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);

            var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(spec);
            if (product is null) throw new ProductNotFoundExceptions(id);
            var productResponse = _mapper.Map<ProductResponse>(product);
            return productResponse;
        }

        public async Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var brandsResponse = _mapper.Map<IEnumerable<BrandTypeResponse>>(brands);
            return brandsResponse;
        }

        public async Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<int, ProductType>().GetAllAsync();
            var typesResponse = _mapper.Map<IEnumerable<BrandTypeResponse>>(types);
            return typesResponse;
        }

    }
}
