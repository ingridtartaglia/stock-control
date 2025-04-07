using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByCodeAsync(string code);
        Task<IEnumerable<Product>> GetAllAsync();
    }
} 