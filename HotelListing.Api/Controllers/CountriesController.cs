﻿using System.Drawing.Text;
using AutoMapper;
using HotelListing.Api.Contracts;
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
        private readonly ICountriesRepository _countriesRepository;
        private readonly IMapper _mapper;
        public CountriesController(IMapper mapper,ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetContries()
        {
            var countries = await _countriesRepository.GetAllAsync();
            var records = _mapper.Map<IList<GetCountryDTO>>(countries);
             
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {

            var country = await _countriesRepository.GetDetails(id);

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

            var country = await _countriesRepository.GetAsync(id);

            if (country==null)
            {
                return NotFound();
            }

            _mapper.Map(updateCountryDto, country);
//          _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _countriesRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await CountryExists(id))
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

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO createCountryDto)
        {
            var country = _mapper.Map<Country>(createCountryDto);

            await _countriesRepository.AddAsync(country);
            
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }  

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _countriesRepository.DeleteAsync(id);
            return NoContent();
        }
}
}
