using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepositary walkRepositary;
        private readonly IMapper mapper;

        public WalksController(IWalkRepositary walkRepositary, IMapper mapper)
        {
            this.walkRepositary = walkRepositary;
            this.mapper = mapper;
        }
        //Create Walk
        // Post: /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if(ModelState.IsValid)
            {
                // map dto to domain
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepositary.CreateAsync(walkDomainModel);

                // map domain to dto - cause sending back to client
                return Ok(mapper.Map<WalkDTO>(walkDomainModel));

            }
            else
            {
                return BadRequest(ModelState);
            }
           

        }

        // get walks 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walkDomainModel = await walkRepositary.GetAllAsync();
            return Ok(mapper.Map<List<WalkDTO>>(walkDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepositary.GetByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            // map domain to dto to send to client
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));



        }

        // PUT : /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                // map dto to domain
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepositary.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                // map from domain to dto 
                return Ok(mapper.Map<WalkDTO>(walkDomainModel));

            }
           
            return BadRequest(ModelState);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomain = await walkRepositary.DeleteAsync(id);
            if (deletedWalkDomain == null)
            {
                return NotFound();
            }

            // map domain to dto
            return Ok(mapper.Map<WalkDTO>(deletedWalkDomain));

        }


    }
}
