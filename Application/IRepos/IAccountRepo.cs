
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepos
{
    public interface IAccountRepo : IGenericRepo<Account>
    {
        void TestMethod(string content);
    }
}
