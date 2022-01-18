using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class inventory
    {
        [Key]
        public int id { get; set; }
        public int pharmacyId { get; set; }
        public int maskId { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal price { get; set; }

        [ForeignKey(nameof(maskId))]
        [InverseProperty("inventory")]
        public virtual mask mask { get; set; }
        [ForeignKey(nameof(pharmacyId))]
        [InverseProperty("inventory")]
        public virtual pharmacy pharmacy { get; set; }
    }
}
