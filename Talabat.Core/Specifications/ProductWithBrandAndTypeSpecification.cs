﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification:BaseSpecification<Product>
    {

        public ProductWithBrandAndTypeSpecification(ProductSpecParams Params) : 
            base(P=>
                  (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search)) &&
                  (!Params.BrandId.HasValue || P.ProductBrandId==Params.BrandId) 
                  &&
                  (!Params.TypeId.HasValue || P.ProductTypeId==Params.TypeId)) 
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOredrBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOredrBy(P=>P.Name); break;   

                }
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1) , Params.PageSize);

        }

        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }

    }
}
