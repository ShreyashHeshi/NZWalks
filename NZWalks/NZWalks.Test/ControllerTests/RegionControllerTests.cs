using Moq;
using NZWalks.API.Repositories;
using AutoMapper;
using NZWalks.API.Controllers;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using Microsoft.Extensions.Logging;



namespace NZWalks.Test
{
    public class RegionControllerTests
    {
        
        private readonly Mock<IRegionRepositary> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<RegionsController>> _mockLogger;
        private readonly RegionsController _controller;

        public RegionControllerTests()
        {
            _mockRepo = new Mock<IRegionRepositary>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<RegionsController>>();
            //_controller = new RegionsController(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

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
               //_mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(regions);                      
               // from Mock(moq), SetUp is used configure regionrepo and come from Moq library and ReturnAsync return region and come from Moq
               _mockMapper.Setup(m => m.Map<List<RegionDTO>>(regions)).Returns(new List<RegionDTO>());
              // mockmapper is automapper obj, map convert from region to regiondto, setup from moq, map from automapper

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result); //Assert is class from xunit
            // istype is mthod from xunit
            // OkObjectResult from Microsoft.AspNetCore.Mvc
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


        [Fact]
        public async Task GetByIdRegion_NotFound()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Region)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task CreateRegionTest()
        {
            // Arrange
            var newRegionDto = new AddRegionRequestDto { Name = "Nelson", Code = "NSN" };
            var newRegion = new Region { Id = Guid.NewGuid(), Name = "Nelson", Code = "NSN" };
            var expectedDto = new RegionDTO { Id = newRegion.Id, Name = "Nelson", Code = "NSN" };

            _mockMapper.Setup(m => m.Map<Region>(newRegionDto)).Returns(newRegion);
            _mockRepo.Setup(repo => repo.CreateAsync(newRegion)).ReturnsAsync(newRegion);
            _mockMapper.Setup(m => m.Map<RegionDTO>(newRegion)).Returns(expectedDto);

            // Act
            var result = await _controller.Create(newRegionDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedRegion = Assert.IsType<RegionDTO>(createdResult.Value);
            Assert.Equal(expectedDto.Name, returnedRegion.Name);
            Assert.Equal(expectedDto.Code, returnedRegion.Code);
        }


        [Fact]
        public async Task UpdateRegionTest()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            var updateDto = new UpdateRegionRequestDto { Name = "Melborne", Code = "MEL" };
            var updatedRegion = new Region { Id = regionId, Name = "Melborne", Code = "MEL" };
            var expectedDto = new RegionDTO { Id = regionId, Name = "Melborne", Code = "MEL" };

            _mockMapper.Setup(m => m.Map<Region>(updateDto)).Returns(updatedRegion);
            _mockRepo.Setup(repo => repo.UpdateAsync(regionId, updatedRegion)).ReturnsAsync(updatedRegion);
            _mockMapper.Setup(m => m.Map<RegionDTO>(updatedRegion)).Returns(expectedDto);

            // Act
            var result = await _controller.Update(regionId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRegion = Assert.IsType<RegionDTO>(okResult.Value);
            Assert.Equal(expectedDto.Name, returnedRegion.Name);
            Assert.Equal(expectedDto.Code, returnedRegion.Code);
        }

        [Fact]
        public async Task UpdateRegionTest_NotFound()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            var updateDto = new UpdateRegionRequestDto { Name = "Kiwi", Code = "KIW" };

            _mockRepo.Setup(repo => repo.UpdateAsync(regionId, It.IsAny<Region>())).ReturnsAsync((Region)null);

            // Act
            var result = await _controller.Update(regionId, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }



        [Fact]
        public async Task DeleteRegionTest()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            var existingRegion = new Region { Id = regionId, Name = "Auckland", Code = "AUK" };
            var expectedDto = new RegionDTO { Id = regionId, Name = "Auckland", Code = "AUK" };

            _mockRepo.Setup(repo => repo.DeleteAsync(regionId)).ReturnsAsync(existingRegion);
            _mockMapper.Setup(m => m.Map<RegionDTO>(existingRegion)).Returns(expectedDto);

            // Act
            var result = await _controller.Delete(regionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<RegionDTO>(okResult.Value);
        }


        [Fact]
        public async Task DeleteRegionTest_NotFound()
        {
            // Arrange
            var regionId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.DeleteAsync(regionId)).ReturnsAsync((Region)null);

            // Act
            var result = await _controller.Delete(regionId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }





















    }
}