using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class RegionRepository(NZWalksDbContext dbContext ) : IRegionRepository
{
    public async Task<List<Region>> GetAllAsync()
    {
       return await dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetRegionByIdAsync(Guid id)
    {
        return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Region> CreateRegionAsync(Region region)
    {
        await dbContext.Regions.AddAsync(region);
        await dbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
    {
        var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRegion == null)
            return null;
        existingRegion.Name = region.Name;
        existingRegion.Code = region.Code;
        existingRegion.RegionImageUrl = region.RegionImageUrl;
        await dbContext.SaveChangesAsync();
        return existingRegion;
    }

    public async Task<Region?> DeleteRegionAsync(Guid id)
    {
        var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRegion == null)
            return null;
        dbContext.Regions.Remove(existingRegion);
        await dbContext.SaveChangesAsync();
        return existingRegion;
    }
}