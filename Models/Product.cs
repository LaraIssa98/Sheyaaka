using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        [JsonIgnore]
        public Store? Store { get; set; }

        [Required]
        public int BrandID { get; set; }
        [ForeignKey("BrandID")]
        [JsonIgnore]
        public Brand? Brand { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        public bool IsDeleted { get; set; } = false;

        

        
    }
}
