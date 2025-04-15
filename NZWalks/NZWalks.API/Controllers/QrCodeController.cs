using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Services;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private readonly IQrCodeService _qrCodeService;

        public QrCodeController(IQrCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        [HttpPost("region")]
        public async Task<IActionResult> GenerateRegionQr([FromBody] RegionQrRequestDto request)
        {
            var result = await _qrCodeService.GenerateQRCodeForRegionAsync(request);
            return Ok(result);
        }
    }
}
