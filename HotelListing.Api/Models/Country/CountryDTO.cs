using HotelListing.Api.Models.Hotel;

namespace HotelListing.Api.Models.Country
{
    public class CountryDTO : BaseCountryDto
    {
        public int Id { get; set; }
        public List<HotelDTO> Hotels { get; set; }
    }
}
 