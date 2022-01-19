using Microsoft.Extensions.Configuration;
using phantom_mask.Data;
using phantom_mask.Models.pharmacy;
using phantom_mask.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace phantom_mask.Repositories
{
    public interface IMaskRepository : IBaseRepository<Mask>
    {

    }
    public class MaskRepository : BaseRepository<Mask>, IMaskRepository
    {
        private readonly IConfiguration _conf;

        public MaskRepository(Context context, IConfiguration conf) : base(context)
        {
            _conf = conf;
        }

        public override Mask GetByID(int id)
        {
            Mask r = _dbSet.SingleOrDefault(a => a.Id == id);
            return r;
        }
    }
}
