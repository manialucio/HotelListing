using HotelListing.Api.Models.Hotel;

namespace HotelListing.Api.Models.Country
{
    public class GetCountryDto:BaseCountryDtoWithId
    {
 
    }

    public class CountryDto:BaseCountryDtoWithId
    {
        public List<HotelDto> Hotels { get; set; }

    }
}
