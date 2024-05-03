using SSRepository.Data;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Master
{
    public interface IOpeningStockRepository: IRepository<TblProdStockDtl>
    {

        string isAlreadyExist(TblProdStockDtlModel tblBankMas, string Mode);
        List<TblProdStockDtlModel> GetList(long FkProdId, int pageSize, int pageNo = 1, string search = "");
        TblProdStockDtlModel GetSingleRecord(long PkID);
        string DeleteRecord(long PKID);
        List<ColumnStructure> ColumnList(string GridName = "");

        string GetByLocationId(Int64 FkLcoationId, Int64 FkLotId);

    }
}
