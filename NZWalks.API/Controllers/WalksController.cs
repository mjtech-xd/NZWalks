using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet]
    public async Task<IActionResult> GetAllWalks()
    {
        var walkDomainModel = await walkRepository.GetAllWalkAsync();
        //Map the Domain Model to Dto
        var walks = mapper.Map<List<WalkDto>>(walkDomainModel);
        return Ok(walks);
    }
}
