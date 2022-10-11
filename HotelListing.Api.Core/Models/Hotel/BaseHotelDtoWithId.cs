using HotelListing.Api.Core.Models.Hotel;

namespace HotelListing.Api.Core.Models.Hotel
{
    public abstract class BaseHotelDtoWithId : BaseHotelDto
    {
        public int Id { get; set; }
    }
}

