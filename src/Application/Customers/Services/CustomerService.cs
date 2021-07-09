using Application.Customers.Model;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<CustomerDto> GetCustomers()
        {
            // Burada automapper kullanabilirsiniz
            var customers = _customerRepository.GetAll.Select(f => new CustomerDto { 
                ExternalId = f.ExternalId,
                Id = f.Id,
                IsActive = f.IsActive,
                Name = f.Name
            });
            return customers.ToList();
        }
    }
}
