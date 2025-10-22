using Store.API.Domain.Entities.Products;
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
        public ProductWithBrandAndTypeSpecifications() : base(null)
        {
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
    }
}
