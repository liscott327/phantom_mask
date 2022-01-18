using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class pharmacy
    {
        public pharmacy()
        {
            inventory = new HashSet<inventory>();
            purchaseHistory = new HashSet<purchaseHistory>();
        }

        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Column(TypeName = "decimal(8, 0)")]
        public decimal cashBalance { get; set; }
        public string openingHours { get; set; }

        [InverseProperty("pharmacy")]
        public virtual ICollection<inventory> inventory { get; set; }
        [InverseProperty("pharmacy")]
        public virtual ICollection<purchaseHistory> purchaseHistory { get; set; }
    }
}
