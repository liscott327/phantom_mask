using System.Collections.Generic;

namespace phantom_mask.Models
{
    public class ImportData
    {
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
    }
}
