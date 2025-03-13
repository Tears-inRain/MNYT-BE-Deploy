using Application.IRepos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class PregnancyStandardRepo : GenericRepo<PregnancyStandard>, IPregnancyStandardRepo
    {
        private readonly AppDbContext _appDbContext;

        public PregnancyStandardRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }

        public async Task<IEnumerable<PregnancyStandard?>> GetByTypeAndPregnancyTypeAsync(string type, string pregnancyType)
        {
            return await _appDbContext.PregnancyStandards
                .Where(x => x.Type == type && x.PregnancyType == pregnancyType)
                .ToListAsync();
        }

    }
}
