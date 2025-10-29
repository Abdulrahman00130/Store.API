using Store.API.Domain.Entities.Products;
using Store.API.Shared.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Specifications.Products
{
    public class ProductsCountSpecifications: BaseSpecifications<int, Product>
    {
        public ProductsCountSpecifications(ProductQueryParameters parameters)
            : base(p =>
            (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId) 
            && 
            (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId)
            &&
            (string.IsNullOrWhiteSpace(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower()))
            )
        {
            
        }
    }
}
