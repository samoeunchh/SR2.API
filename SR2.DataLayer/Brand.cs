using System;
using System.ComponentModel.DataAnnotations;

namespace SR2.DataLayer
{
    public class Brand
    {
        [Key]
        public Guid BrandId { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Brand name field is required")]
        public string BrandName { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
