using System;
using System.Collections.Generic;

#nullable disable

namespace BankAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
