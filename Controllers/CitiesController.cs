using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CityGuideApi.Data;
using CityGuideApi.Dtos;
using CityGuideApi.Models;
using System.Collections.Generic;

namespace CityGuideApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;

        public CitiesController(IAppRepository appRepository, IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityForListDto>> GetCities()
        {
            var cities = _appRepository.GetCities();
            var citiesToReturn = _mapper.Map<IEnumerable<CityForListDto>>(cities);
            return Ok(citiesToReturn);
        }

        [HttpGet("detail")]
        public ActionResult<CityForDetailDto> GetCityById(int id)
        {
            var city = _appRepository.GetCityById(id);
            var cityToReturn = _mapper.Map<CityForDetailDto>(city);
            return Ok(cityToReturn);
        }

        [HttpGet("photos")]
        public ActionResult<IEnumerable<Photo>> GetPhotosById(int cityId)
        {
            var photos = _appRepository.GetPhotosByCity(cityId);
            return Ok(photos);
        }

        [HttpPost("add")]
        public ActionResult AddCity([FromBody] CityForCreationDto cityForCreationDto)
        {
            var cityToAdd = _mapper.Map<City>(cityForCreationDto);
            _appRepository.Add(cityToAdd);
            _appRepository.SaveAll();
            return Ok(cityToAdd);
        }
    }
}
