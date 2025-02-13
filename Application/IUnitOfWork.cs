using Application.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUnitOfWork
    {
        ISubjectRepo SubjectRepo { get; }

        public Task<int> SaveChangesAsync();
    }
}
