using AutoMapper;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities.Products;
using Store.API.Services.Abstractions.Products;
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
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.GetRepository<int, Product>().GetAllAsync();
            var productsResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productsResponse;
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(id);
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
