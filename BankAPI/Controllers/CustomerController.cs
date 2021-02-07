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
                AccountBalance accBal = _context.AccountBalances.Where(x => x.AccountId == transaction.AccountId).FirstOrDefault();   
                accBal.Balance = (accBal.Balance + transaction.Amount);
                
                _context.SaveChanges();
               
                return Ok($"Amount {transaction.Amount} deposited Successfully. Available Balance in your Account is : {accBal.Balance}");

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
                AccountBalance accBal = _context.AccountBalances.Where(x => x.AccountId == transaction.AccountId).FirstOrDefault();
                
                if(accBal.Balance < transaction.Amount)
                {
                    return Ok("Insufficient funds.");
                }
                else
                {
                    accBal.Balance = (accBal.Balance - transaction.Amount);
                    _context.SaveChanges();
                    return Ok($"Amount {transaction.Amount} withdrawn successfully. Available Balance in your Account is : {accBal.Balance}");
                }

            }
             catch (Exception)
             {
                   return Ok("Error occured while doing withdraw.");
             }
        }
    }
}
