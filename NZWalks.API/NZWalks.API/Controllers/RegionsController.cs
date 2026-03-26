using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> GetAllRegions()
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
    }
}
