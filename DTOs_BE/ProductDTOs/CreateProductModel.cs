using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_BE.ProductDTOs
{
    public class CreateProductModel
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string MinAge { get; set; }
        public string MaxAge { get; set; }
        public string SafetyFeature { get; set; }
        public double Rating { get; set; }
        public string RecommendedBy { get; set; }
        public string ImageUrl { get; set; }
        public string Brand { get; set; }
        public bool IsActive { get; set; }
        public string ProductType { get; set; }
    }
}
