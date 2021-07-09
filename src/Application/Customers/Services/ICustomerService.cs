using Application.Customers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Services
{
    public interface ICustomerService
    {
        List<CustomerDto> GetCustomers();
    }
}
