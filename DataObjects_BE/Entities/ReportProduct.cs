using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataObjects_BE.Entities;

public class ReportProduct
{
    [Key]
    public Guid ReportProductId { get; set; }

    [ForeignKey("Report")]
    public Guid ReportId { get; set; }
    public virtual Report Report { get; set; }
        
    [ForeignKey("ProductList")]
    public Guid ProductListId { get; set; }
    public virtual ProductList ProductList { get; set; }
}