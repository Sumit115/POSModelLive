
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Transaction
{
    public interface ISalesChallanRepository : ITranBaseRepository
    {
        List<ColumnStructure> ColumnList(string GridName = "");
    }
}
