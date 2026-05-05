using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,
    NZWalksDbContext dbContext) : IImageRepository
{
    public async Task<Image> UploadImage(Image image)
    {
        var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "images", $"{image.FileName}{image.FileExtension}");
        
        //Upload image to Local Path 
        await using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(stream);
        
        //http://localhost:5000/images/image.jpg
        var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/images/{image.FileName}{image.FileExtension}";
        image.FilePath = urlFilePath;
        
        //Add Image to Images Table
        await dbContext.Images.AddAsync(image);
        await dbContext.SaveChangesAsync();
        return image;
    }
}