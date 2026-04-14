using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
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
            if (!ValidateAddRegionsAsync(addRegionRequest))
            {
                return BadRequest(ModelState);          
            }
            

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

            if (!ValidateUpdateRegionsAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

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

        #region Private Methods

        private bool ValidateAddRegionsAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest==null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"{nameof(addRegionRequest)} Add Region Data is Required.");

            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),$"{nameof(addRegionRequest.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} cannot be null or empty or white space.");
            }

            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} cannot be leass than or equal to zero.");
            }

            if (addRegionRequest.lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.lat), $"{nameof(addRegionRequest.lat)} cannot be leass than or equal to zero.");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)} cannot be leass than or equal to zero.");
            }

            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} cannot be leass than zero.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateRegionsAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $"{nameof(updateRegionRequest)} Add Region Data is Required.");

            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} cannot be null or empty or white space.");
            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} cannot be leass than or equal to zero.");
            }

            //if (updateRegionRequest.lat <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateRegionRequest.lat), $"{nameof(updateRegionRequest.lat)} cannot be leass than or equal to zero.");
            //}
            //if (updateRegionRequest.Long <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateRegionRequest.Long), $"{nameof(updateRegionRequest.Long)} cannot be leass than or equal to zero.");
            //}

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} cannot be leass than zero.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
