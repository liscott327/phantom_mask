using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class purchaseHistory
    {
        [Key]
        public int id { get; set; }
        public int userId { get; set; }
        public int pharmacyId { get; set; }
        public int maskId { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal transactionAmount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime transactionDate { get; set; }

        [ForeignKey(nameof(maskId))]
        [InverseProperty("purchaseHistory")]
        public virtual mask mask { get; set; }
        [ForeignKey(nameof(pharmacyId))]
        [InverseProperty("purchaseHistory")]
        public virtual pharmacy pharmacy { get; set; }
        [ForeignKey(nameof(userId))]
        [InverseProperty("purchaseHistory")]
        public virtual user user { get; set; }
    }
}
