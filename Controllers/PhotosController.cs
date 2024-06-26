using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CityGuideApi.Data;
using CityGuideApi.Dtos;
using CityGuideApi.Helpers;
using CityGuideApi.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace CityGuideApi.Controllers
{
    [Route("api/cities/{cityId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly Cloudinary _cloudinary; 

        public PhotosController(IAppRepository appRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _appRepository = appRepository;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddPhotoForCity(int cityId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            var city = _appRepository.GetCityById(cityId);

            if (city == null)
            {
                return BadRequest("Could not find the city");
            }
             
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (currentUserId != city.UserId)
            {
                return Unauthorized();
            }

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            photo.City = city;

            if (!city.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }

            city.Photos.Add(photo);

            if (_appRepository.SaveAll())
            {
                var photoReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public ActionResult GetPhoto(int id)
        {
            var photoFromDb = _appRepository.GetPhoto(id);
            
            if (photoFromDb == null)
            {
                return NotFound();
            }

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromDb);
            return Ok(photo);
        }
    }
}
