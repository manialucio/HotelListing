using AutoMapper;
using HotelListing.Api.Core.Contracts;
using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            this._context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            var country = await _context.Countries.Include(h => h.Hotels).FirstOrDefaultAsync(q => q.Id == id);
            return country;
        }
    }
}
