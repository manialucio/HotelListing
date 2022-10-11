using HotelListing.Api.Core.Models.Hotel;

namespace HotelListing.Api.Core.Models.Country
{
    public class GetCountryDto:BaseCountryDtoWithId
    {
 
    }

    public class CountryDto:BaseCountryDtoWithId
    {
        public List<HotelDto> Hotels { get; set; }

    }
}
