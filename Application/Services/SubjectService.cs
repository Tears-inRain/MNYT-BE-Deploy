using Application.IServices;
using Application.ViewModels.Subject;
using AutoMapper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SubjectService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(SubjectAddVM subjectVM)
        {
            if (subjectVM.TuitionFee < 100)
            {
                throw new Exception("Price must be greater than 100");
            }
            Account item = _mapper.Map<Account>(subjectVM);
            await _unitOfWork.SubjectRepo.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<SubjectVM>> GetAllAsync()
        {
            var list = await _unitOfWork.SubjectRepo.GetAllAsync();

            //var filterList = list.Where(x => x.TuitionFee < 100);

            var convertedList = _mapper.Map<List<SubjectVM>>(list);
            return convertedList;
        }

        public async Task<SubjectVM> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.SubjectRepo.GetAsync(id);
            var convertedItem = _mapper.Map<SubjectVM>(item);
            return convertedItem;
        }
    }
}
