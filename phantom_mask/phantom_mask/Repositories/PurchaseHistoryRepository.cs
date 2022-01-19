using Microsoft.Extensions.Configuration;
using phantom_mask.Data;
using phantom_mask.Models.pharmacy;
using System.Linq;

namespace phantom_mask.Repositories
{
    public interface IPurchaseHistoryRepository : IBaseRepository<PurchaseHistory>
    {

    }
    public class PurchaseHistoryRepository : BaseRepository<PurchaseHistory>, IPurchaseHistoryRepository
    {
        private readonly IConfiguration _conf;

        public PurchaseHistoryRepository(Context context, IConfiguration conf) : base(context)
        {
            _conf = conf;
        }

        public override PurchaseHistory GetByID(int id)
        {
            PurchaseHistory r = _dbSet.SingleOrDefault(a => a.Id == id);
            return r;
        }
    }
}
