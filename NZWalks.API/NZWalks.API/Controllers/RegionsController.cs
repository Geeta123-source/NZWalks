using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllRegionsAsync()
        {
          var regions = await regionRepository.GetAllAsync();

            //return DTO region

            //var regionsDto = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDto = new Models.DTO.Region()
            //    {
            //        Id=region.Id,
            //        Name=region.Name,
            //        Code=region.Code,
            //        Area=region.Area,
            //        lat=region.lat,
            //        Long=region.Long,
            //        Population=region.Population,
            //    };
            //    regionsDto.Add(regionDto);
            //});

            var regionDto = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionDto);

        }

        [HttpGet]
        [Route("{Id:Guid}")]
        [ActionName("GetRegionsAsync")]
        public async Task<IActionResult> GetRegionsAsync(Guid Id)
        {
            var regions = await regionRepository.GetAsync(Id);
            if (regions == null)
            {
                return NotFound();
            }

            var regionDto= mapper.Map<Models.DTO.Region>(regions);
            return Ok(regionDto);
        }

        [HttpPost]

        public async Task<IActionResult> AddRegionsAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request Dto to domain model

            var region = new Models.Domain.Region()
            {
                Code=addRegionRequest.Code,
                Name=addRegionRequest.Name,
                lat=addRegionRequest.lat,
                Long=addRegionRequest.Long,
                Area=addRegionRequest.Area,
                Population=addRegionRequest.Population

            };

            //Pass details to repository
            
            region = await regionRepository.AddAsync(region);

            //Convert back to dto

            var regionDto = new Models.DTO.Region()
            {
               
                Code=region.Code,
                Name=region.Name,
                lat=region.lat,
                Long=region.Long,
                Area=region.Area,
                Population=region.Population

            };

            return CreatedAtAction(nameof(GetRegionsAsync), new { id = region.Id }, regionDto);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid Id)
        {
            //Get region from databse

            var region =await regionRepository.DeleteAsync(Id);

            if (region == null)
            {
                return NotFound();
            }

            //Convert response back to home

            var regionDto = new Models.DTO.Region()
            {
                Code = region.Code,
                Name = region.Name,
                lat = region.lat,
                Long = region.Long,
                Area = region.Area,
                Population = region.Population
            };

            return Ok(regionDto);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync(Guid Id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                lat = updateRegionRequest.lat,
                Long = updateRegionRequest.Long,
                Area = updateRegionRequest.Area,
                Population = updateRegionRequest.Population

            };

            region = await regionRepository.UpdateAsync(Id, region);
            if (region == null)
            { 
                return NotFound();
            }

            var regionDto = new Models.DTO.Region()
            {
                Code = region.Code,
                Name = region.Name,
                lat = region.lat,
                Long = region.Long,
                Area = region.Area,
                Population = region.Population
            };

            return Ok(regionDto);

        }
            
     }
}
