using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositories;

public interface IWalkRepository
{
    Task<Walk> CreateAsync(Walk walk);
    Task<List<Walk>> GetAllWalkAsync(string? filterOn =null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber =1, int pageSize = 100);
    Task<Walk?> GetWalkByIdAsync(Guid id);
    Task<Walk?>UpdateAsync(Guid id, Walk walk);
    Task<Walk?> DeleteAsync(Guid id);
}