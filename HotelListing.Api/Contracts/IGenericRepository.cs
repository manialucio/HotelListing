using HotelListing.Api.Data;
using HotelListing.Api.Models;

namespace HotelListing.Api.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);
        Task<List<T>> GetAllAsync();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);   

        Task<bool> Exists(int id); 
    }


}
