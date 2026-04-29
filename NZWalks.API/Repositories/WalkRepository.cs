using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositories;

public class WalkRepository(NZWalksDbContext dbContext) : IWalkRepository
{
    public async Task<Walk> CreateAsync(Walk walk)
    {
        await dbContext.Walks.AddAsync(walk);
        await dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<List<Walk>> GetAllWalkAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
        bool isAscending = true)
    {
        var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
        
        //Filtering
        if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if(filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.Name.ToLower().Contains(filterQuery.ToLower())); 
            }

            else if (filterOn.Equals("description", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.Description.ToLower().Contains(filterQuery.ToLower()));
            }

            else if (filterOn.Equals("lengthInKm", StringComparison.OrdinalIgnoreCase))
            {
                if(double.TryParse(filterQuery, out var length))
                {
                    walks = walks.Where(x => x.LengthInKm >= length);
                }
            }
        }
        //Sorting 
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.Name) :  walks.OrderByDescending(x => x.Name);
            }
            else if(sortBy.Equals("lengthInKm", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
            }
        }
        return await walks.ToListAsync();
        //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
    }

    public async Task<Walk?> GetWalkByIdAsync(Guid id)
    {
        return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        if(existingWalk == null)
            return  null;
        existingWalk.Name = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.LengthInKm = walk.LengthInKm;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.DifficultyId = walk.DifficultyId;
        existingWalk.RegionId = walk.RegionId;
        
        await dbContext.SaveChangesAsync();
        return existingWalk;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var result = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        if (result == null)
            return null;
        dbContext.Walks.Remove(result);
        await dbContext.SaveChangesAsync();
        return result;
    }
}