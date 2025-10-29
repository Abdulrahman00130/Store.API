using Store.API.Domain.Entities.Products;
using Store.API.Shared.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Specifications.Products
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductWithBrandAndTypeSpecifications(ProductQueryParameters parameters)
            : base(p =>
            (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId) 
            && 
            (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId)
            &&
            (string.IsNullOrWhiteSpace(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower()))
            )
        {
            ApplyPagination(parameters.PageIndex, parameters.PageSize);
            ApplySorting(parameters.Sort);
            ApplyIncludes();
        }

        public ProductWithBrandAndTypeSpecifications(int id) : base(p => p.Id == id)
        {
            ApplyIncludes();
            
        }

        private void ApplyIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "pricedesc":
                        AddOrderByDescending(p => p.Price);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
        }
    }
}
