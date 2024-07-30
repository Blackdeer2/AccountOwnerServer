using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
   public class AccountRepository : RepositoryBase<Account>, IAccountRepository
   {
      public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

      public IEnumerable<Account> AccountsByOwner(Guid ownerId)
      {
         return FindByCondition(ac => ac.OwnerId.Equals(ownerId)).ToList();
      }
   }
}
