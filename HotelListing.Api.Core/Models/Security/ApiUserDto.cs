using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.Core.Models.Security
{
    public class ApiUserDto:LoginDto
    {
        [Required]
        public string FirtsName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}