﻿
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ILocalityRepository : IRepository<TblLocalityMas>
    {
         List<ColumnStructure> ColumnList(string GridName = "");

        string isAlreadyExist(LocalityModel tblBankMas, string Mode);
        List<LocalityModel> GetList(int pageSize, int pageNo = 1, string search = "", int FkAreaId = 0);
        object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FkAreaId = 0);

        LocalityModel GetSingleRecord(long PkID);

        string DeleteRecord(long PKID);
    }
}
