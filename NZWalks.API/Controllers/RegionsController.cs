using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController(NZWalksDbContext dbContext) : ControllerBase
{
    //Get All Regions
    [HttpGet]
    public IActionResult GetAll()
    {
        var regions = dbContext.Regions.ToList();
        return Ok(regions);
    }
    
    //Get Region by Id
    [HttpGet]
    [Route("{id:Guid}")]
    public IActionResult GetById([FromRoute]Guid id)
    {
        //var result = dbContext.Regions.Find(id);
        var result = dbContext.Regions.FirstOrDefault(x => x.Id == id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
}