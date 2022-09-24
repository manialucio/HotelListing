using HotelListing.Api.Models.Hotel;

namespace HotelListing.Api.Models.Hotel
{
    public abstract class BaseHotelDtoWithId : BaseHotelDto
    {
        public int Id { get; set; }
    }
}

