namespace phantom_mask.Models
{
    public class Query
    {
        public class Store
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string OpeningHours { get; set; }
        }

        public class GoodDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        public class TransactionAmountRank
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public decimal TotalTransactionAmount { get; set; }
        }
    }
}
