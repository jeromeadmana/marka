using marka_api.Models;

namespace marka_api.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(Guid id);
        Task<Customer> CreateCustomer(Customer customer);
        Task<bool> DeleteCustomer(Guid id);
    }
}
