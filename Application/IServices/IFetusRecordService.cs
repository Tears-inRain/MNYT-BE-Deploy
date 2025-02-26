using Application.ViewModels.FetusRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IFetusRecordService
    {
        Task AddAsync(FetusRecordAddVM fetusRecordAddVM);
        Task UpdateAsync(FetusRecordVM fetusRecordVM);
        Task<FetusRecordVM> GetByIdAsync(int id);
        void DeleteAsync(int id);
        void SoftDelete(int id);
    }
}
