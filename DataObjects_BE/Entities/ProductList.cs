using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE.Entities
{
    public partial class ProductList
    {
        [Key]
        public Guid ProductListId { get; set; }
        //public Guid? ReportId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string MinAge { get; set; }
        public string MaxAge { get; set; }
        public string SafetyFeature { get; set; }
        public double Rating { get; set; }
        public String RecommendedBy { get; set; }
        public string ImageUrl { get; set; }
        public string Brand { get; set; }
        public bool IsActive { get; set; }
        
                
        public string ProductType { get; set; }
        
        public virtual ICollection<ReportProduct> ReportProducts { get; set; }

    }
}
