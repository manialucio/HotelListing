using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data;
using AutoMapper;
using HotelListing.Api.Contracts;
using HotelListing.Api.Models.Hotel;
using HotelListing.Api.Models;
using Microsoft.AspNetCore.OData.Query;

namespace HotelListing.Api.Controllers
{
    [Route("api/v{version:ApiVersion}/hotels")]
    [ApiController]
    [ApiVersion("2.0")]
    public class HotelsV2Controller : ControllerBase
    {
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IMapper _mapper;

        public HotelsV2Controller(IHotelsRepository hotelsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _hotelsRepository = hotelsRepository;
        }



        // GET: api/Hotels
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _hotelsRepository.GetAllAsync();
            var hotelist = _mapper.Map<List<HotelDto>>(hotels);
            return Ok(hotelist);
        }


        // GET: api/Hotels/?StartIndex=0&pageSize=25&PAgeNumber=1
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<PagedResult<HotelDto>>> GetPagedHotels([FromQuery] QueryParameters queryParameters)
        {
            var pagedResult = await _hotelsRepository.GetAllAsync<HotelDto>(queryParameters);
            return Ok(pagedResult);
        }


        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }
            var retVal = _mapper.Map<HotelDto>(hotel);

            return retVal;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelToUpdate)
        {
            var hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            // automapper "sporca" l'oggetto a EF mette lo stato a modify
            _mapper.Map(hotelToUpdate, hotel);

            try
            {
                await _hotelsRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createHotel)
        {
            var hotel = _mapper.Map<Hotel>(createHotel);
            await _hotelsRepository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!await _hotelsRepository.Exists(id))
            {
                return NotFound();

            }
            await _hotelsRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _hotelsRepository.Exists(id);
        }
    }
}
