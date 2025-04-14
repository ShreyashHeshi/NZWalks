using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CQRS.Query;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfExportController : ControllerBase
    {

        private readonly IPdfGenerator pdfGenerator;
        private readonly IMediator mediator;
        private readonly ILogger<PdfExportController> logger;

        public PdfExportController(IPdfGenerator pdfGenerator, IMediator mediator, ILogger<PdfExportController> logger)
        {
            this.pdfGenerator = pdfGenerator;
            this.mediator = mediator;
            this.logger = logger;
        }

        // Generate Region Data PDF
        [HttpGet("regions")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GenerateRegionPdf([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            logger.LogInformation("Generating PDF for paginated regions...");

            var query = new GetAllRegionsQuery { Page = page, PageSize = pageSize };
            var response = await mediator.Send(query);

            if (response == null || response.Regions == null || !response.Regions.Any())
                return NotFound("No region data found to export.");

            var pdfBytes = await pdfGenerator.GenerateRegionsPdfAsync(response.Regions);
            return File(pdfBytes, "application/pdf", "RegionsReport.pdf");
        }


        // Generate Custom Input PDF
        [HttpPost("custom")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GenerateCustomPdf([FromBody] CustomPdfRequestDto request)
        {
            try
            {
                logger.LogInformation($"Generating custom PDF with title: {request.Title}");

                var pdfBytes = await pdfGenerator.GenerateCustomPdfAsync(request);
                return File(pdfBytes, "application/pdf", "CustomInputReport.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to genrate Pdf");

                throw new Exception($"{ex.Message}");
            }
        }
    }
}
