using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // get all regions
        // https://localhost:7084/api/Regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from database - domain models
           var regionsDomain = dbContext.Regions.ToList();

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
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);

            // second way to find id by Linq method FirstOrDefault
            // get domain model from database
            var regionsDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionsDomain == null)
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
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // map dto to domain model
            var regionDomainModel = new Region
            {
                Code= addRegionRequestDto.Code,
                Name= addRegionRequestDto.Name,
                RegionImageUrl= addRegionRequestDto.RegionImageUrl
            };

            // use domain model to create region 
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

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
        public IActionResult Update([FromRoute] Guid id,  [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // check if region exists
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x=>x.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to Domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name= updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl= updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

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
        public IActionResult Delete([FromRoute] Guid id)
        {
            // find region in database
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x=>x.Id==id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

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
