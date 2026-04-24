using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController(NZWalksDbContext dbContext) : ControllerBase
{
    //Get All Regions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //Get data from Database - Domain model
        var regionDomain =await dbContext.Regions.ToListAsync();
        
        //Map Domain Model to DTO
        var regionDto =new List<RegionDto>();
        foreach (var region in regionDomain)
        {
            regionDto.Add(new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            });
        }
        return Ok(regionDto);
    }
    
    //Get Region by Id
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute]Guid id)
    {
        //var result = dbContext.Regions.Find(id);
        //Get region Domain model from database
        var regionDomain =await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        
        if (regionDomain == null)
            return NotFound();
        //Map Region Domain Model to DTO
        var regionDto = new RegionDto()
        {
            Id = regionDomain.Id,
            Code = regionDomain.Code,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl,
        };
            
        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        //Map or convert DTO to Domain Model
        var regionDomainModel = new Region()
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl,
        };
        //Use Domain Model to create Region
        await dbContext.Regions.AddAsync(regionDomainModel);
        await dbContext.SaveChangesAsync();
        
        //Map Domain model back to Dto
        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl,
        };
        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto )
    {
        //Check if region exists
        var regionDomainModel =await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if(regionDomainModel == null)
            return NotFound();
        //Map Dto to Domain Model
        regionDomainModel.Code = updateRegionRequestDto.Code;
        regionDomainModel.Name = updateRegionRequestDto.Name;
        regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
        
        await dbContext.SaveChangesAsync();
        //Convert Domain model to DTo
        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,
        };
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (regionDomainModel == null)
            return NotFound();
        dbContext.Remove(regionDomainModel);
        await dbContext.SaveChangesAsync();
        return Ok();

    }
}