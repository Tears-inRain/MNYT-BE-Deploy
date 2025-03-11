using Application.IServices;
using Application.ViewModels.ScheduleTemplate;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ScheduleTemplateService : IScheduleTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleTemplateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(ScheduleTemplateAddVM scheduleTemplateAddVM)
        {
            var schedule = _mapper.Map<ScheduleTemplate>(scheduleTemplateAddVM);
            await _unitOfWork.ScheduleTemplateRepo.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.ScheduleTemplateRepo.GetAsync(id);
            _unitOfWork.ScheduleTemplateRepo.Delete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<ScheduleTemplateVM>> GetAllAsync()
        {
            var items = await _unitOfWork.ScheduleTemplateRepo.GetAllAsync();
            var result = _mapper.Map<IList<ScheduleTemplateVM>>(items);
            return result;
        }

        public async Task<ScheduleTemplateVM> GetAsync(int id)
        {
            var item = await _unitOfWork.ScheduleTemplateRepo.GetAsync(id);
            var result = _mapper.Map<ScheduleTemplateVM>(item);
            return result;
        }

        public async Task SoftDeleteAsync(int id)
        {
            var itemToDelete = await _unitOfWork.ScheduleTemplateRepo.GetAsync(id);
            _unitOfWork.ScheduleTemplateRepo.SoftDelete(itemToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ScheduleTemplateVM scheduleTemplateVM)
        {
            var itemToUpdate = _mapper.Map<ScheduleTemplate>(scheduleTemplateVM);
            _unitOfWork.ScheduleTemplateRepo.Update(itemToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
