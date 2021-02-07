using BankAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly BankContext _context;
        public CustomerController(BankContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("GetCustomer")]
        public async Task<ActionResult<Customer>> GetCustomer()
        {
            try
            {
                string emailAddress = HttpContext.User.Identity.Name;

                var customer = await _context.Customers.Include(x => x.Accounts)
                                                            .ThenInclude(x => x.AccountBalances)
                                                       .Where(customer => customer.Email == emailAddress).FirstOrDefaultAsync();
                if (customer == null)
                {
                    return NotFound();
                }
                return customer;
            }
            catch (Exception)
            {
                return Ok("Error occured.");
            }
            
        }

        [HttpPut("DepositAmount")]
        public IActionResult DepositAmount(Transaction transaction)
        {
            try
            {
                _context.Customers.FromSqlRaw("EXEC PROC_DEPOSIT @CUSTOMERID={0},@AMOUNT={1}", transaction.CustomerId, transaction.Amount).ToListAsync().Result.FirstOrDefault();

                Customer cus = _context.Customers.Include(x => x.Accounts)
                                        .ThenInclude(x => x.AccountBalances)
                                        .Where(x => x.CustomerId == transaction.CustomerId).FirstOrDefault();

                BalanceDetails balance = new BalanceDetails();
                balance.CustomerId = cus.CustomerId;
                var Account = cus.Accounts.Where(x => x.CustomerId == balance.CustomerId).FirstOrDefault();
                balance.AccountId = Account.AccountId;
                balance.Name = cus.Name;
                var AccountNumber = cus.Accounts.Where(x => x.AccountId == balance.AccountId).FirstOrDefault();
                balance.AccountNumber = AccountNumber.AccountNumber;
                var balanceDetails = Account.AccountBalances.Where(x => x.AccountId == balance.AccountId).FirstOrDefault();
                balance.Balance = balanceDetails.Balance;

                return Ok(balance);
            }
            catch (Exception)
            {
                return Ok("Error occured while doing Deposit.");
            }
        }

        [HttpPut("WithdrawAmount")]
        public IActionResult WithdrawAmount(Transaction transaction)
        {
            try
            {
                var checkTransaction = _context.Customers.FromSqlRaw("EXEC PROC_WITHDRAW @CUSTOMERID={0},@AMOUNT={1}", transaction.CustomerId, transaction.Amount).ToListAsync().Result.FirstOrDefault();

                if (checkTransaction.Name == "InsufficientFunds")
                {
                    return Ok("Insufficient Funds..... Please withdraw valid amount.");
                }
                else
                {
                    Customer cus = _context.Customers.Include(x => x.Accounts)
                                   .ThenInclude(x => x.AccountBalances)
                                   .Where(x => x.CustomerId == transaction.CustomerId).FirstOrDefault();
                    BalanceDetails balance = new BalanceDetails();
                    balance.CustomerId = cus.CustomerId;
                    var Account = cus.Accounts.Where(x => x.CustomerId == balance.CustomerId).FirstOrDefault();
                    balance.AccountId = Account.AccountId;
                    balance.Name = cus.Name;
                    var AccountNumber = cus.Accounts.Where(x => x.AccountId == balance.AccountId).FirstOrDefault();
                    balance.AccountNumber = AccountNumber.AccountNumber;
                    var balanceDetails = Account.AccountBalances.Where(x => x.AccountId == balance.AccountId).FirstOrDefault();
                    balance.Balance = balanceDetails.Balance;
                    return Ok(balance);

                }
            }
            catch (Exception)
            {
                return Ok("Error occured while doing withdraw.");
            }
        }
    }
}
