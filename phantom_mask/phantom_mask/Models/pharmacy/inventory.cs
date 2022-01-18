using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class Inventory
    {
        [Key]
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public int MaskId { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(MaskId))]
        [InverseProperty("Inventory")]
        public virtual Mask Mask { get; set; }
        [ForeignKey(nameof(PharmacyId))]
        [InverseProperty("Inventory")]
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
