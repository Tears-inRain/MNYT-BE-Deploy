using Application.IRepos;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class SubjectRepo : GenericRepo<Account>, IAccountRepo
    {
        private readonly AppDbContext _appDbContext;
        public SubjectRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }

        public void TestMethod(string content)
        {
            Console.WriteLine(content);
        }
    }
}
