using BankingAPI.Models;
using BankingAPI.DTOs;

namespace BankingAPI.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> GetCustomerById(int id);
        Task<IEnumerable<CustomerResponseDto>> GetAllCustomers();
        Task<CustomerResponseDto> AddCustomer(CustomerRequestDto customerDto);
        Task<CustomerResponseDto> UpdateCustomer(int id, CustomerRequestDto customerDto);
        Task<CustomerResponseDto> DeleteCustomer(int id);
    }
}
