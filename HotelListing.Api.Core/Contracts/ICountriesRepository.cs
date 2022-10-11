using HotelListing.Api.Data;

namespace HotelListing.Api.Core.Contracts
{
    public interface ICountriesRepository: IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }

}
