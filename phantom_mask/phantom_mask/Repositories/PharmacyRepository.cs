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
    public interface IPharmacyRepository : IBaseRepository<Pharmacy>
    {

    }
    public class PharmacyRepository : BaseRepository<Pharmacy>, IPharmacyRepository
    {
        private readonly IConfiguration _conf;

        public PharmacyRepository(Context context, IConfiguration conf) : base(context)
        {
            _conf = conf;
        }

        public override Pharmacy GetByID(int id)
        {
            Pharmacy r = _dbSet.SingleOrDefault(a => a.Id == id);
            return r;
        }


    }
}
