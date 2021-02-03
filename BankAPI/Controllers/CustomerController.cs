using BankAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            using(var context = new BankContext())
            {
                //Get all customers
                //return context.Customers.ToList();

                //Get customer by Id
                //return context.Customers.Where(cust => cust.CustomerId == 1).ToList();

                //Add Customer
                //Customer cust = new Customer();
                //cust.Name = "Manju";
                //cust.Email = "manju@gmail.com";
                //cust.Password = "manju234";

                //context.Customers.Add(cust);
                //context.SaveChanges();

                //return context.Customers.Where(cust => cust.Email == "manju@gmail.com").ToList();

                //Update customer
                //Customer cust = context.Customers.Where(cust => cust.Name == "Manju").FirstOrDefault();
                //cust.Email = "manju54321@gmail.com";
                //context.SaveChanges();

                //return context.Customers.Where(cust => cust.Name == "Manju").ToList();

                //Remove customer

                Customer cust = context.Customers.Where(cust => cust.Name == "Manju").FirstOrDefault();
                if(cust != null)
                {
                    context.Remove(cust);
                    context.SaveChanges();
                }
                
                return context.Customers.Where(cust => cust.Name == "Manju").ToList();


            }
        }
    }
}
