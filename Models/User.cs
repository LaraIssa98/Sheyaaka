using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Text.Json.Serialization;

namespace Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        [MaxLength(255)]
        public string ProfilePictureURL { get; set; }

        /*public int? StoreID { get; set; }

        [ForeignKey("StoreID")]
        [JsonIgnore]
        public Store? Store { get; set; }*/
    }




}
