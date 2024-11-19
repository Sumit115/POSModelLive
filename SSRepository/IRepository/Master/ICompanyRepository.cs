
using SSRepository.Data;
using SSRepository.Models;

namespace SSRepository.IRepository.Master
{
    public interface ICompanyRepository : IRepository<TblCompany>
    {
        CompanyModel GetSingleRecord();        
    }
}
