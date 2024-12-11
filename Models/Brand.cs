using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Models
{
    public class Brand
    {
        [Key]
        public int BrandID { get; set; }

        [Required]
        [MaxLength(100)]
        public string BrandName { get; set; }
       

        //public ICollection<Store>? Stores { get; set; }
        
        public ICollection<Product>? Products { get; set; }
    }



}
