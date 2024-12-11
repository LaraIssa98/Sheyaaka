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
    public class Address
    {
        [Key]
        public int AddressID { get; set; }

        [Required]
        public int StoreID { get; set; }
        [ForeignKey("StoreID")]
        [JsonIgnore]
        public Store? Store { get; set; }

        [Required]
        [MaxLength(255)]
        public string AddressLine { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string State { get; set; }

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; }

        public bool IsActive { get; set; } = true;        
    }

}
