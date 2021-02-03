using System;
using System.Collections.Generic;

#nullable disable

namespace BankAPI.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountBalances = new HashSet<AccountBalance>();
        }

        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public int AccountNumber { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<AccountBalance> AccountBalances { get; set; }
    }
}
