using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Entity;

namespace Repositories
{
    public interface IRecordTypeRepository : IRepositoryBase<RecordType>
    {
        Task<List<RecordType>?> GetAllRecordType(int pageNumber, int pageSize);

    }
}
