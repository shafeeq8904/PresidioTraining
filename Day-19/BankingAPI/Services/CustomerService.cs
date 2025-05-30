using BankingAPI.DTOs;
using BankingAPI.Interfaces;
using BankingAPI.Models;
using BankingAPI.Repositories;

namespace BankingAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<int, Customer> _customerRepository;

        public CustomerService(IRepository<int, Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponseDto> AddCustomer(CustomerRequestDto customerDto)
        {
            var customer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email
            };

            var result = await _customerRepository.Add(customer);

            return ToResponseDto(result);
        }

        public async Task<CustomerResponseDto> DeleteCustomer(int id)
        {
            var result = await _customerRepository.Delete(id);
            return ToResponseDto(result);
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAll();
            return customers.Select(c => ToResponseDto(c)).ToList();
        }

        public async Task<CustomerResponseDto> GetCustomerById(int id)
        {
            var customer = await _customerRepository.Get(id);
            return ToResponseDto(customer);
        }

        public async Task<CustomerResponseDto> UpdateCustomer(int id, CustomerRequestDto customerDto)
        {
            var updatedCustomer = new Customer
            {
                CustomerId = id,
                Name = customerDto.Name,
                Email = customerDto.Email
            };

            var result = await _customerRepository.Update(id, updatedCustomer);
            return ToResponseDto(result);
        }

        private CustomerResponseDto ToResponseDto(Customer customer)
        {
            return new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Accounts = customer.Accounts?.Select(a => new AccountResponseDto
                {
                    AccountId = a.AccountId,
                    AccountNumber = a.AccountNumber,
                    Balance = a.Balance,
                    CustomerId = a.CustomerId
                }).ToList() ?? new List<AccountResponseDto>()
            };
        }
    }
}
