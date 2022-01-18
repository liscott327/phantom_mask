using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace phantom_mask.Models.pharmacy
{
    public partial class User
    {
        public User()
        {
            PurchaseHistory = new HashSet<PurchaseHistory>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal CashBalance { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<PurchaseHistory> PurchaseHistory { get; set; }
    }
}
