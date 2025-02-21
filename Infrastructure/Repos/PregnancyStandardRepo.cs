using Application.IRepos;
using Domain.Entities;
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
    }
}
