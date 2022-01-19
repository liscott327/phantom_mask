using Microsoft.Extensions.Configuration;
using phantom_mask.Data;
using phantom_mask.Models.pharmacy;
using System.Linq;

namespace phantom_mask.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {

    }
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IConfiguration _conf;

        public UserRepository(Context context, IConfiguration conf) : base(context)
        {
            _conf = conf;
        }

        public override User GetByID(int id)
        {
            User r = _dbSet.SingleOrDefault(a => a.Id == id);
            return r;
        }
    }
}
