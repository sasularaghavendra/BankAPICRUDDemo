using System;
using System.Collections.Generic;

namespace BankAPI.Models
{
    public partial class AccountBalance
    {
        
        public int AccBalanceId { get; set; }
        public int AccountId { get; set; }
        public int Balance { get; set; }
        public virtual Account Account { get; set; }
    }
}
