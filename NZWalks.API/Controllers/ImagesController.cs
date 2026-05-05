using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController(IImageRepository imageRepository) : ControllerBase
{
    //Post: /api/Images/upload
    [HttpPost]
    [Route("Upload")]
    public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
    {
        ValidateUploadRequest(request);
        if (ModelState.IsValid)
        {
            //Convert Dto to Domain model 
            var imageDomainModel = new Image()
            {
                File =  request.File,
                FileName = request.FileName,
                FileDescription = request.FileDescription,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
            };
            //Use repository to Upload
            await imageRepository.UploadImage(imageDomainModel);
            return Ok(imageDomainModel);
        }
        return BadRequest(ModelState);
    }

    private void ValidateUploadRequest(ImageUploadRequestDto request)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png"};
        if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
        {
            ModelState.AddModelError("File", "Only .jpg, .jpeg, and .png files are allowed.");
        }

        if (request.File.Length > 10485760)
        {
            ModelState.AddModelError("File", "File size is too large");
        }
    }
}