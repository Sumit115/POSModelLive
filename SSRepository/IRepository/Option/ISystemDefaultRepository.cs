using SSRepository.Data;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Options
{
    public interface ISystemDefaultRepository 
    {

        string GetSingleKey(string Key);
        Task<SysDefaults> GetSingleRecord();

        Task SaveBaseAsync(SysDefaults model);
    }
}
