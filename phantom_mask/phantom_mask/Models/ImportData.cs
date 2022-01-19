using System;
using System.Collections.Generic;

namespace phantom_mask.Models
{
    public class ImportData
    {
        #region Pharmacies
        public class Pharmacies
        {
            public string name { get; set; }
            public decimal cashBalance { get; set; }
            public string openingHours { get; set; }
            public List<Good> masks { get; set; }
        }

        public class Good
        {
            public string name { get; set; }
            public decimal price { get; set; }
        }
        #endregion

        #region UserData
        public class Users
        {
            public string name { get; set; }
            public decimal cashBalance { get; set; }
            public List<History> purchaseHistories { get; set; }
        }

        public class History
        {
            public string pharmacyName { get; set; }
            public string maskName { get; set; }
            public decimal transactionAmount { get; set; }
            public DateTime transactionDate { get; set; }
        }
        #endregion
    }
}
