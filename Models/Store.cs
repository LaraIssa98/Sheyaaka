using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Store
    {
        [Key]
        public int StoreID { get; set; }
        public int? UserID { get; set; }

        [ForeignKey("UserID")]
        [JsonIgnore]
        public User? Users { get; set; }

        [Required]
        [MaxLength(100)]
        public string StoreName { get; set; }

        public string LogoURL { get; set; }

        public int? BrandID { get; set; }
        [ForeignKey("BrandID")]
        [JsonIgnore]
        public Brand? Brand { get; set; }

        public bool IsActive { get; set; } = true;

        
        
        /*[JsonIgnore]
        public ICollection<Address>? Addresses { get; set; }
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }*/
    }

}
