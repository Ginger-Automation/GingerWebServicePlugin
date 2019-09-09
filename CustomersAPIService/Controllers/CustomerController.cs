using CustomersAPIService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace CustomersAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        CustomerManager customerManager = new CustomerManager();

        // GET api/customer
        [HttpGet]
        public ActionResult<List<Customer>> Get()
        {
            return CustomerManager.GetCustomers();
        }

        // GET api/customer/5
        [HttpGet("{id:int}")]
        public ActionResult<Customer> Get(int id)
        {
            Customer customer = CustomerManager.GetCustomerByID(id.ToString());
            return customer;
        }

        // GET api/customer/name
        [HttpGet("id:string")]
        public ActionResult<Customer> Get(string id)
        {
            Customer customer = CustomerManager.GetCustomerByName(id);
            return customer;
        }

        // POST api/customer
        [HttpPost]
        public ActionResult<object> CustomerAction()
        {
            string status = null;
            //if (customer == null)
            //{
            //    status = "Empty Customer Information - failed to add";
            //}
            //else
            //{
            //    CustomerManager.AddCustomer(customer);
            //    status = "Customer Added";
            //}
            Customer c = new Customer()
            {
                CustomerAddress = Request.Form["CustomerAddress"],
                CustomerID = Request.Form["CustomerID"],
                CustomerName= Request.Form["CustomerName"]
            };

            return c;
        }



        [HttpPut("put")]
        public ActionResult<List<Customer>> Put()
        {
            HttpContext.Response.Headers.Add("MessageType", "Put");
            return CustomerManager.GetCustomers();
        }



        [HttpPost("add")]
        public ActionResult<string> Add()
        {
            var body = new StreamReader(Request.Body);
            //The modelbinder has already read the stream and need to reset the stream index
    
            var requestBody = body.ReadToEnd();
            return requestBody;
        }


        // PUT api/customer/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/customer/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            bool status = CustomerManager.RemoveCustomerByID(id.ToString());
            if (status == true)
            {
                return "Customer Deleted";
            }
            else
            {
                return "Customer Not Found";
            }
        }
    }
}
