using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalksController(IWalkRepository walkRepository, IMapper mapper) : ControllerBase
{
    //Create Walks
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        //Map Dto to Domain Model
        var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
        await walkRepository.CreateAsync(walkDomainModel);
        //Map Domain Model to DTO
        var walkDto = mapper.Map<WalkDto>(walkDomainModel);
        return Ok(walkDto);
    }

    //Get All Walks
    //Get: //api/walks?filterOn=name&filterQuery=Track
    [HttpGet]
    public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery]  string? filterQuery)
    {
        var walkDomainModel = await walkRepository.GetAllWalkAsync(filterOn, filterQuery);
        //Map the Domain Model to Dto
        var walks = mapper.Map<List<WalkDto>>(walkDomainModel);
        return Ok(walks);
    }

    //Get Walk by Id
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
    {
        var walkDomainModel = await walkRepository.GetWalkByIdAsync(id);
        if (walkDomainModel == null)
            return NotFound();
        //Map the domain model to Dto
        var walk = mapper.Map<WalkDto>(walkDomainModel);
        return Ok(walk);
    }

    //Update Walk By Id
    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
    {
        //Map to domain model
        var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
        walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
        if (walkDomainModel == null)
            return NotFound();
        //Map Domain Model to Dto
        var walk = mapper.Map<WalkDto>(walkDomainModel);
        return Ok(walk);
    }

    //Delete by Id
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
        if (deletedWalkDomainModel == null)
            return NotFound();
        //Map the Domain mode to Dto
        var deletedWalk = mapper.Map<WalkDto>(deletedWalkDomainModel);
        return Ok(deletedWalk);
    }
}