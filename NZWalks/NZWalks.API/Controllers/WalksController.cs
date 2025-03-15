using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Collections.Specialized;

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
        // /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10                
        // after walks first query parameter is filter on query param
        // filter on basically say what column do you filter on and after that receive query user wants to filter on
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber=1, [FromQuery] int pageSize=1000)
        {
            var walkDomainModel = await walkRepositary.GetAllAsync(filterOn,filterQuery, sortBy, isAscending ?? true, pageNumber,pageSize); // pass query parameters here imp
            // isAscending is nullable boolean but repo accept only boolean value so if nullable then do isAscending true
            // so sending is always true if its null value
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
