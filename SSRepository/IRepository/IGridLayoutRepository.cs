using SSRepository.Data;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository
{
    public interface IGridLayoutRepository : IRepository<TblGridStructer>
    {
        TblGridStructer GetSingleRecord(long FkFormId,string GridName, List<ColumnStructure> columns);

    }
}
