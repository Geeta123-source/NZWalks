using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync()
        {

            //Fetch data from database :Domian walk
            var walksDomain = await walkRepository.GetAllAsync();

            // Convert domain walks to dto walks

            var walksDTO = mapper.Map<List<Models.Domain.Walk>>(walksDomain);

            //Return Response

            return Ok(walksDTO);


        }
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk domain from database
            var walkdomain = await walkRepository.GetAsync(id);
            if (walkdomain == null)
            {
                return NotFound();
            }


            //convert domain to dto

            var WalkDto = mapper.Map<Models.DTO.Walk>(walkdomain);

            return Ok(WalkDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {

            //convert DTO to Domain 

            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };



            //Pass Domain object to repository to persist back to dto

            await walkRepository.AddAsync(walkDomain);

            //send dto response to client

            var walkDTO = new Models.DTO.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] Models.DTO.UpdateWalkRequestcs updateWalkRequest)
        {
            //convert DTO TO Domain
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId

            };

            //pass details to repository. Get domain object in response

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            if (walkDomain == null)
            {
                return NotFound();
            }
            //convert back domain to dto}

            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId

            };

            return Ok(walkDTO);

        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkdomain = await walkRepository.DeleteAsync(id);
            if (walkdomain == null)
            {
                return NotFound();
            }
            var WalkDto = mapper.Map<Models.DTO.Walk>(walkdomain);

            return Ok(WalkDto);
        }
    }
}