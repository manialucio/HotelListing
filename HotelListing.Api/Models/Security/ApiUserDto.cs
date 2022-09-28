using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.Models.Security
{
    public class ApiUserDto
    {
        [Required]
        public string FirtsName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage ="Password > 6 caratteri  e < 15",MinimumLength =6)]

        public string Password { get; set; }
    }
}