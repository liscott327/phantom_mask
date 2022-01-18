using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class Pharmacy
    {
        public Pharmacy()
        {
            Inventory = new HashSet<Inventory>();
            PurchaseHistory = new HashSet<PurchaseHistory>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(8, 0)")]
        public decimal CashBalance { get; set; }
        public string OpeningHours { get; set; }

        [InverseProperty("Pharmacy")]
        public virtual ICollection<Inventory> Inventory { get; set; }
        [InverseProperty("Pharmacy")]
        public virtual ICollection<PurchaseHistory> PurchaseHistory { get; set; }
    }
}
