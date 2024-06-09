using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.Entity;

namespace Repositories.Impl
{
    public class RecordTypeRepository : RepositoryBase<RecordType>, IRecordTypeRepository
    {
        public RecordTypeRepository(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<RecordType>?> GetAllRecordType(int pageNumber, int pageSize)
        {
            List<RecordType> recordTypes = await Paging(pageNumber, pageSize);
            return recordTypes;
        }
    }
}
