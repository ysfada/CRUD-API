
using Application.Customers.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _customerService.GetCustomers();
            return Ok(data);
        }
    }
}