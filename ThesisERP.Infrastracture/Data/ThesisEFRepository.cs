using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Infrastracture.Data
{
    public class ThesisEFRepository<T> : BaseRepository<T> where T : class
    {
        public ThesisEFRepository(DatabaseContext dbContext) : base(dbContext)
        {

        }
    }
}
