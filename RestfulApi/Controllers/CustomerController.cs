using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Models;
using RestfulApi.Data;
using System.Net;
using RestfulApi.Interfaces;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase, ICustomerController
    {
        private readonly ApiContext _context;

        public CustomerController(ApiContext context)
        {
            _context = context;
        }

        // Get all customers
        [HttpGet]
        public JsonResult GetAll()
        {
            var customers = _context.Customers.ToList();

            return new JsonResult(Ok(customers));
        }

        // Get a specific customer by ID
        [HttpGet]
        public JsonResult GetByID(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customer));
        }

        // Get costumers by DNI (Note to Leo: Normally the DNI is unique, but since in this case I did not use the DNI as the ID identifying each customer)
        // If the DNI is unique, this function should be modified.
        [HttpGet]
        public JsonResult GetByDNI(string dni)
        {
            var customers = _context.Customers.Where(customer => customer.DNI == dni);

            if (customers.Count() == 0)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customers));
        }

        // Get customers with the same name
        [HttpGet]
        public JsonResult GetByName(string name)
        {
            var customers = _context.Customers.Where(customer => customer.Name == name);

            if (customers.Count() == 0)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customers));
        }

        // Get customers that live in the same State
        [HttpGet]
        public JsonResult GetByState(string state)
        {
            var customers = _context.Customers.Where(customer => customer.State == state);

            if (customers.Count() == 0)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customers));
        }

        // Get customers that live in the same City
        [HttpGet]
        public JsonResult GetByCity(string city)
        {
            var customers = _context.Customers.Where(customer => customer.City == city);

            if (customers.Count() == 0)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customers));
        }

        // Get customer with e-mail (Assuming each customer has only one e-mail, and multiple customers can't share the same e-mail)
        [HttpGet]
        public JsonResult GetByEmail(string email)
        {
            var customers = _context.Customers.Where(customer => customer.Email == email);

            if (customers.Count() == 0)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customers));
        }

        // Get customers by phone/mobile number
        [HttpGet]
        public JsonResult GetByPhoneNumber(string phoneNumber)
        {
            var customers = _context.Customers.Where(customer => customer.Phone == phoneNumber || customer.Mobile == phoneNumber);

            if (customers.Count() == 0)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customers));
        }

        // Get customers by address
        [HttpGet]
        public JsonResult GetByAddress(string address)
        {
            var customers = _context.Customers.Where(customer => customer.Address == address);

            if (customers.Count() == 0)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(customers));
        }

        // Create/Edit a customer
        [HttpPost]
        public JsonResult CreateEdit(Customer customer) // Using the same function to Create a new customer or Edit an existing one
        {
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.Find(customer.Id);

                if (customerInDb == null)
                {
                    return new JsonResult(NotFound());
                }

                // I'm updating each property individually so that Entity Framework keeps track of the changes
                customerInDb.Name = customer.Name;
                customerInDb.DNI = customer.DNI;
                customerInDb.Address = customer.Address;
                customerInDb.Phone = customer.Phone;
                customerInDb.Mobile = customer.Mobile;
                customerInDb.Email = customer.Email;
                customerInDb.State = customer.State;
                customerInDb.City = customer.City;
            }
            _context.SaveChanges();

            return new JsonResult(Ok(customer));
        }

        // Delete a customer by ID
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return new JsonResult(NotFound());
            }

            _context.Customers.Remove(customer);
            
            _context.SaveChanges(true);
            
            return new JsonResult(NoContent());
        }
    }
}
