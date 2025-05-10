
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Transaction
{
    public interface ISalesReplacementRepository : ITranBaseRepository
    {
        List<ColumnStructure> ColumnList(string GridName = "");
    }
}
