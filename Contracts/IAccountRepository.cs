using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
   public interface IAccountRepository: IRepositoryBase<Account>
   {
      IEnumerable<Account> AccountsByOwner(Guid ownerId);
      Account GetAccountById(Guid accountId);
      public void CreateAccount(Account account);
      public void UpdateAccount(Account account);
      public void DeleteAccount(Account account);
   }
}
