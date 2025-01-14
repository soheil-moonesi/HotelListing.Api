using System.Drawing.Text;
using AutoMapper;
using HotelListing.Api.Data;
using HotelListing.Api.Models.Country;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;
        public CountriesController(HotelListingDbContext context,IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetContries()
        {
            var countries = await _context.Countries.ToListAsync();
            var records = _mapper.Map<IList<GetCountryDTO>>(countries);
             
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        { 
            var country = await _context.Countries
                .Include(q => q.Hotels).FirstOrDefaultAsync(q=>q.Id==id);

        //    var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<CountryDTO>(country);

            return Ok(countryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid Record Id");
            }

            var country = await _context.Countries.FindAsync(id);
            if (country==null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDto, country);
//          _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(country => country.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO createCountryDto)
        {
            var country = _mapper.Map<Country>(createCountryDto);

           await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }  

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }
}
}
