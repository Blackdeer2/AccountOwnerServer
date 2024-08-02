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

      public Account GetAccountById(Guid accountId)
      {
         return FindByCondition(ac => ac.Id.Equals(accountId))
               .FirstOrDefault();
      }

      public IEnumerable<Account> AccountsByOwner(Guid ownerId)
      {
         return FindByCondition(ac => ac.OwnerId.Equals(ownerId)).ToList();
      }

      public void CreateAccount(Account account)
      {
         Create(account);
      }

      public void UpdateAccount(Account account)
      {
         Update(account);
      }

      public void DeleteAccount(Account account)
      {
         Delete(account);
      }
   }
}
