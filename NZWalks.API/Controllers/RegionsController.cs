using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController(IRegionRepository regionRepository, IMapper mapper) : ControllerBase
{
    //Get All Regions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //Get data from Database - Domain model
        var regionDomain = await regionRepository.GetAllAsync();

        //Map domain models to DTO
        var regionDto = mapper.Map<List<RegionDto>>(regionDomain);
        return Ok(regionDto);
    }

    //Get Region by Id
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        //var result = dbContext.Regions.Find(id);
        //Get region Domain model from database
        var regionDomain = await regionRepository.GetRegionByIdAsync(id);

        if (regionDomain == null)
            return NotFound();
        var regionDto = mapper.Map<RegionDto>(regionDomain);

        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
        //Use Domain Model to create Region
        regionDomainModel = await regionRepository.CreateRegionAsync(regionDomainModel);

        //Map Domain model back to Dto
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);
        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id,
        [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
        //Check if region exists
        regionDomainModel = await regionRepository.UpdateRegionAsync(id, regionDomainModel);
        if (regionDomainModel == null)
            return NotFound();
        var regionDto = mapper.Map<RegionDto>(regionDomainModel);
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel = await regionRepository.DeleteRegionAsync(id);
        if (regionDomainModel == null)
            return NotFound();

        var regionDto = mapper.Map<RegionDto>(regionDomainModel);
        return Ok(regionDto);
    }
}