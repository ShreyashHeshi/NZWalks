using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;
using AutoMapper;
using NZWalks.API.CustomActionFilter;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using MediatR;
using NZWalks.API.CQRS.Query;
using NZWalks.API.CQRS.Command;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IMediator mediator;
        private readonly IRegionRepositary regionRepositary;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        // imapper is interface from automapper
        public RegionsController(NZWalksDbContext dbContext,IMediator mediator, IRegionRepositary regionRepositary, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
            this.regionRepositary = regionRepositary;
            this.mapper = mapper;
            this.logger = logger;
        }

        /*public RegionsController(IRegionRepositary regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.regionRepositary = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        } 
        */

        // get all regions
        // https://localhost:7084/api/Regions
        [HttpGet]
        [Authorize(Roles ="Reader")]
        // task is return type of async. every return type is wrapped inside a task
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 9)
        {

            var query = new GetAllRegionsQuery { Page = page, PageSize = pageSize };
            var result = await mediator.Send(query);
            return Ok(result);
            logger.LogInformation("Get all action method was involked");


            //Get data from database - domain models
            // await is used to ensure that this is async call
            //var regionsDomain = await dbContext.Regions.ToListAsync(); // this dbcontext method
            // tolistAsync mthod come from Microsoft.EntityFrameworkCore pacakge

            // now we call repositary method
            //var regionsDomain = await regionRepositary.GetAllAsync();

            // map domain models to dtos
            //var regionsDto = new List<RegionDTO>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDTO()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //   });
            //}

            //logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");       
            // convert domain object into json object using system.text.json so $ is used

            // map domain models to dtos
            // var regionsDto = mapper.Map<List<RegionDTO>>(regionsDomain);

            // return dto
            // return Ok(regionsDto);
        }

        // Get region by id
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await mediator.Send(new GetRegionByIdQuery(id));
            if (region == null) return NotFound();
            return Ok(region);


            //var region = dbContext.Regions.Find(id);

            // second way to find id by Linq method FirstOrDefault
            // get domain model from database
            //var regionsDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            /*var regionsDomain = await regionRepositary.GetByIdAsync(id);
            if (regionsDomain == null)
            {
                return NotFound();
            }*/

            // map/convert region model into region dto
            /*var regionsDto = new RegionDTO
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageUrl = regionsDomain.RegionImageUrl

            };*/

            //return Ok(mapper.Map<RegionDTO>(regionsDomain));


        }

        // to create new region in database

        [HttpPost]
        //[ValidateModel]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            var region = await mediator.Send(new CreateRegionCommand
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            });

            var regionDto = mapper.Map<RegionDTO>(region);
            logger.LogWarning($"📝 Region added at {DateTime.Now}");
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);


            /*// Map DTO to Command
            var createRegionCommand = mapper.Map<CreateRegionCommand>(addRegionRequestDto);

            // Send the mapped command to Mediator
            var region = await mediator.Send(createRegionCommand);

            // Convert the result to DTO before returning
            return CreatedAtAction(nameof(GetById), new { id = region.Id }, mapper.Map<RegionDTO>(region));*/









            //if(ModelState.IsValid)  // modelstate comes from asp.netcoremvc.modelbinding.modelstatedictionary
            //{
            // map dto to domain model
            /*var regionDomainModel = new Region
            {
                Code= addRegionRequestDto.Code,
                Name= addRegionRequestDto.Name,
                RegionImageUrl= addRegionRequestDto.RegionImageUrl
            };*/
            // imp var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);



            // use domain model to create region 
            // await dbContext.Regions.AddAsync(regionDomainModel);
            // await dbContext.SaveChangesAsync();

            // use domain model to create region
            // imp regionDomainModel = await regionRepositary.CreateAsync(regionDomainModel);

            // map domain model back to DTO
            /*var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };*/
            // imp var regionDto = mapper.Map<RegionDTO>(regionDomainModel);



            // return 201 reponse created
            // imp return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

            /*}
            else
            {
                return BadRequest(ModelState);
            }*/



        }

        // PUT - Update domain in database
        [HttpPut]
        [Route("{id:Guid}")]
        //[ValidateModel]
        [Authorize(Roles = "Writer,Reader")]    
        public async Task<IActionResult> Update([FromRoute] Guid id,  [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            var updateRegion = await mediator.Send(new UpdateRegionCommand(id, updateRegionRequestDto));
            if (updateRegion == null) return NotFound(); 
            return Ok(updateRegion);




            //if (ModelState.IsValid)
            //{
                // map dto to domain model
                /*var regionDomainModel = new Region
                {
                    Code= updateRegionRequestDto.Code,
                    Name= updateRegionRequestDto.Name,
                    RegionImageUrl= updateRegionRequestDto.RegionImageUrl
                };*/
                // imp var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);



                // check if region exists
                //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
                /* imp regionDomainModel = await regionRepositary.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }*/



                // convert domain to dto
                /*var regionDto = new RegionDTO
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };*/
                /* imp var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

                return Ok(regionDto);*/

            /*}
            else
            {
                return BadRequest(ModelState);
            }*/

           

        }

        //Delete Region
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            var deletedRegion = await mediator.Send(new DeleteRegionCommand(id));

            if (deletedRegion == null) return NotFound();
            return Ok(deletedRegion);





            // find region in database
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            /* imp var regionDomainModel = await regionRepositary.DeleteAsync(id);


            if (regionDomainModel == null)
            {
                return NotFound();
            }*/


            // map domain model to dto for return deleted region back
            /*var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };*/
            /* imp var regionDto = mapper.Map<RegionDTO> (regionDomainModel);

            return Ok(regionDto); */


        }
        



    }
}
