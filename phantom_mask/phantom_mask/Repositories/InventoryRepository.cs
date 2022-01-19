using Microsoft.Extensions.Configuration;
using phantom_mask.Data;
using phantom_mask.Models.pharmacy;
using phantom_mask.Repositories;
using System.Linq;

namespace phantom_Inventory.Repositories
{
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {

    }
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        private readonly IConfiguration _conf;

        public InventoryRepository(Context context, IConfiguration conf) : base(context)
        {
            _conf = conf;
        }

        public override Inventory GetByID(int id)
        {
            Inventory r = _dbSet.SingleOrDefault(a => a.Id == id);
            return r;
        }
    }
}
