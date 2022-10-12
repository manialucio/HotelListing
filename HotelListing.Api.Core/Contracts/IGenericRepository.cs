using HotelListing.Api.Data;
using HotelListing.Api.Core.Models;

namespace HotelListing.Api.Core.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);
        Task<TResult?> GetAsync<TResult>(int? id);
        Task<List<T>> GetAllAsync();
        Task<List<TResult>> GetAllAsync<TResult>();

        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<TResult> AddAsync<TSource,TResult>(TSource source);
        Task UpdateAsync<TSource>(int id, TSource source);

        Task DeleteAsync(int id);   

        Task<bool> Exists(int id); 
    }


}
