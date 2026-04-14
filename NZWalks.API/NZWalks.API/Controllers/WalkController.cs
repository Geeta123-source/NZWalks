using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalkController(IWalkRepository walkRepository, IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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

            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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

        #region Private Methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} Can not be empty.");
                return false;
            }

            
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is Required.");
            }

            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater than zero.");
            }

            var region =await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is Invalid.");
            }

            var walkDifficulty =await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is Invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequestcs updateWalkRequestcs)
        {
            if (updateWalkRequestcs == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequestcs), $"{nameof(updateWalkRequestcs)} Can not be empty.");
                return false;
            }


            if (string.IsNullOrWhiteSpace(updateWalkRequestcs.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequestcs.Name), $"{nameof(updateWalkRequestcs.Name)} is Required.");
            }

            if (updateWalkRequestcs.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequestcs.Length), $"{nameof(updateWalkRequestcs.Length)} should be greater than zero.");
            }

            var region = await regionRepository.GetAsync(updateWalkRequestcs.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequestcs.RegionId), $"{nameof(updateWalkRequestcs.RegionId)} is Invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequestcs.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequestcs.WalkDifficultyId), $"{nameof(updateWalkRequestcs.WalkDifficultyId)} is Invalid.");
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