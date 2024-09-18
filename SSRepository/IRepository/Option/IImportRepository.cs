using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Option
{
    public interface IImportRepository:IBaseRepository
    {
        string SaveData(List<TranDetails> JsonData);

    }
}
