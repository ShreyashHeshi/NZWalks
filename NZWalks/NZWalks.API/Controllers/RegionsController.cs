using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepositary regionRepositary;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepositary regionRepositary)
        {
            this.dbContext = dbContext;
            this.regionRepositary = regionRepositary;
        }

        // get all regions
        // https://localhost:7084/api/Regions
        [HttpGet]

        // task is return type of async. every return type is wrapped inside a task
        public async Task<IActionResult> GetAll()
        {
            //Get data from database - domain models
            // await is used to ensure that this is async call
            //var regionsDomain = await dbContext.Regions.ToListAsync(); // this dbcontext method
            // tolistAsync mthod come from Microsoft.EntityFrameworkCore pacakge

            // now we call repositary method
            var regionsDomain = await regionRepositary.GetAllAsync();

            // map domain models to dtos
            var regionsDto = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });

            }
            // return dto
            return Ok(regionsDto);
        }

        // Get region by id

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);

            // second way to find id by Linq method FirstOrDefault
            // get domain model from database
            //var regionsDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionsDomain = await regionRepositary.GetByIdAsync(id);
            if (regionsDomain == null)
            {
                return NotFound();
            }

            // map/convert region model into region dto

            var regionsDto = new RegionDTO
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageUrl = regionsDomain.RegionImageUrl

            };

            return Ok(regionsDto);


        }

        // to create new region in database

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // map dto to domain model
            var regionDomainModel = new Region
            {
                Code= addRegionRequestDto.Code,
                Name= addRegionRequestDto.Name,
                RegionImageUrl= addRegionRequestDto.RegionImageUrl
            };

            // use domain model to create region 
           // await dbContext.Regions.AddAsync(regionDomainModel);
           // await dbContext.SaveChangesAsync();

            // use domain model to create region
            regionDomainModel = await regionRepositary.CreateAsync(regionDomainModel);

            // map domain model back to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };


            // return 201 reponse created
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        // PUT - Update domain in database
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,  [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            // map dto to domain model
            var regionDomainModel = new Region
            {
                Code= updateRegionRequestDto.Code,
                Name= updateRegionRequestDto.Name,
                RegionImageUrl= updateRegionRequestDto.RegionImageUrl
            };


            // check if region exists
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);

            regionDomainModel = await regionRepositary.UpdateAsync(id, regionDomainModel);
            
            if (regionDomainModel == null)
            {
                return NotFound();
            }

           

            // convert domain to dto
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);

        }

        //Delete Region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // find region in database
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);

            var regionDomainModel = await regionRepositary.DeleteAsync(id);


            if (regionDomainModel == null)
            {
                return NotFound();
            }


            // map domain model to dto for return deleted region back
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);


        }
        



    }
}
