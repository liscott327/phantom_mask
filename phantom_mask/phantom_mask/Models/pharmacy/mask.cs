using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class Mask
    {
        public Mask()
        {
            Inventory = new HashSet<Inventory>();
            PurchaseHistory = new HashSet<PurchaseHistory>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [InverseProperty("Mask")]
        public virtual ICollection<Inventory> Inventory { get; set; }
        [InverseProperty("Mask")]
        public virtual ICollection<PurchaseHistory> PurchaseHistory { get; set; }
    }
}
