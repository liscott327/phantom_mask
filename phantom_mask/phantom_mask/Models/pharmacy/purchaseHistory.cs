using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class PurchaseHistory
    {
        public PurchaseHistory()
        {
            this.TransactionDate = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PharmacyId { get; set; }
        public int MaskId { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal TransactionAmount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }

        [ForeignKey(nameof(MaskId))]
        [InverseProperty("PurchaseHistory")]
        public virtual Mask Mask { get; set; }
        [ForeignKey(nameof(PharmacyId))]
        [InverseProperty("PurchaseHistory")]
        public virtual Pharmacy Pharmacy { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("PurchaseHistory")]
        public virtual User User { get; set; }
    }
}
