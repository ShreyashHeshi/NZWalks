using Moq;
using NZWalks.API.Repositories;
using AutoMapper;
using NZWalks.API.Controllers;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;



namespace NZWalks.Test
{
    public class RegionControllerTests
    {
        private readonly Mock<IRegionRepositary> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly RegionsController _controller;

        public RegionControllerTests()
        {
            _mockRepo = new Mock<IRegionRepositary>();
            _mockMapper = new Mock<IMapper>();
            _controller = new RegionsController(null, _mockRepo.Object, _mockMapper.Object);

        }

        [Fact]
        public async Task GetAllRegionsTest()
        {
            var regions = new List<Region> {

                new Region
                {
                     Id = Guid.NewGuid(),
                     Code = "NSN",
                     Name="Nelson"
                }};
               _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(regions);                      
               // from Mock(moq), SetUp is useed onfigure regionrepo and come from Moq library and ReturnAsync return region and come from Moq
               _mockMapper.Setup(m => m.Map<List<RegionDTO>>(regions)).Returns(new List<RegionDTO>());
              // mockmapper is automapper obj, setup convert from region to regiondto, setup and map from moq

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result); //Assert is class from xunit
            // istype is mthod from xunit
            Assert.IsType<List<RegionDTO>>(okResult.Value);

        }

        [Theory]
        [InlineData("906CB139-415A-4BBB-A174-1A1FAF9FB1F6")]
        [InlineData("F7248FC3-2585-4EFB-8D1D-1C555F4087F6")]
        public async Task GetByIdRegionTest(string id)
        {
            // Arrange
            var regionId = Guid.Parse(id);
            var regions = new Region
            {

                Id = Guid.NewGuid(),
                Code = "NSN",
                Name = "Nelson"
            };
            _mockRepo.Setup(repo=>repo.GetByIdAsync(regionId)).ReturnsAsync(regions);
            _mockMapper.Setup(m=>m.Map<RegionDTO>(regions)).Returns(new RegionDTO());

            // Act
            var result = await _controller.GetById(regionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<RegionDTO>(okResult.Value);

        }












    }
}