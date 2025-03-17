using Application.Services.IServices;
using Application.ViewModels.FetusRecord;
using Application.ViewModels.Pregnancy;
using Application.ViewModels.ScheduleUser;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ScheduleUserService : IScheduleUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ScheduleUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(ScheduleUserAddVM scheduleUserAddVM)
        {
            ScheduleUser schedule = _mapper.Map<ScheduleUser>(scheduleUserAddVM);
            await _unitOfWork.ScheduleUserRepo.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.ScheduleUserRepo.GetAsync(id);
            _unitOfWork.ScheduleUserRepo.Delete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<ScheduleUserVM>> GetAllAsync()
        {
            var items = await _unitOfWork.ScheduleUserRepo.GetAllAsync();
            var result = _mapper.Map<IList<ScheduleUserVM>>(items);

            return result;
        }

        public async Task<ScheduleUserVM> GetAsync(int id)
        {
            var item = await _unitOfWork.ScheduleUserRepo.GetAsync(id);
            var result = _mapper.Map<ScheduleUserVM>(item);
            return result;
        }

        public async Task SoftDelete(int id)
        {
            var schedule = await _unitOfWork.ScheduleUserRepo.GetAsync(id);
            _unitOfWork.ScheduleUserRepo.SoftDelete(schedule);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ScheduleUserVM scheduleUserVM)
        {
            var itemToUpdate = _mapper.Map<ScheduleUser>(scheduleUserVM);
            _unitOfWork.ScheduleUserRepo.Update(itemToUpdate);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<ScheduleUserVM>> GetAllByPregnancyIdAsync(int id)
        {
            var query = _unitOfWork.ScheduleUserRepo.GetAllQueryable().Where(x => x.PregnancyId == id);
            var list = await query.ToListAsync();
            return _mapper.Map<IList<ScheduleUserVM>>(list);
        }
    }
}
