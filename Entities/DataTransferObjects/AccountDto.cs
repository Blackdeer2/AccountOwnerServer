using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
   public class AccountDto
   {
      public Guid Id { get; set; }
      public DateTime DateCreated { get; set; }
      public string? AccountType { get; set; }
   }
}
