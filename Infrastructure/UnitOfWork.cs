using Application;
using Application.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly AppDbContext _context;

        public readonly ISubjectRepo _subjectRepo;

        public UnitOfWork(AppDbContext context, ISubjectRepo subjectRepo)
        {
            _context = context;
            _subjectRepo = subjectRepo;
        }

        public ISubjectRepo SubjectRepo => _subjectRepo;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
