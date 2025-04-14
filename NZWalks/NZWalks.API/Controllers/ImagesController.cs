using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CQRS.Command;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepositary imageRepositary;
        private readonly IMediator mediator;

        public ImagesController(IImageRepositary imageRepositary, IMediator mediator)
        {
            this.imageRepositary = imageRepositary;
            this.mediator = mediator;
        }

        // POST: api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)  //are going to sned file as part of file data so use FromForm
        {

            var imageUpload = await mediator.Send(new UploadImageCommand(request));
            if (imageUpload == null)
            {
                return NotFound();
            } 
            return Ok(imageUpload); 

            //ValidateFileUpload(request);
            //if(ModelState.IsValid)
            //{
                // convert DTO into Domain model
            /*    var imageDomainModel = new Image
                {
                    File=request.File,
                    FileExtension=Path.GetExtension(request.File.FileName), //Path from System.io.Path
                    FileSizeInBytes=request.File.Length,
                    FileName=request.FileName,
                    FileDescription=request.FileDescription,
                };

                //User repositary to upload image
                await imageRepositary.Upload(imageDomainModel);

                return Ok(imageDomainModel); */
            //}
            //return BadRequest(ModelState);

        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtension.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported file extension");
                // addModelError takes two properties one is key i.e. file and error msg
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }
    }
}
