﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepositary : IWalkRepositary
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepositary(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

       

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
           //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            //include method takes information of difficulty and region by difficulty id and region id
            // aslo do this way .Include(x=>x.Difficulty)


            // IQuerable of walk
            var walks= dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable(); 

            //filtering
            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery)==false )
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks=walks.Where(x=>x.Name.Contains(filterQuery));
                }
            }

            // sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks= isAscending ?  walks.OrderBy(x=>x.Name): walks.OrderByDescending(x=>x.Name) ;
                }else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks= isAscending ? walks.OrderBy(x=>x.LenghtInKm): walks.OrderByDescending(x=>x.LenghtInKm) ;
                }
            }

            // pagination
            var skipResults = (pageNumber-1)*pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty").Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk == null)
            {
                return null;
            } 

            existingWalk.Name=walk.Name;
            existingWalk.Description=walk.Description;
            existingWalk.LenghtInKm= walk.LenghtInKm;
            existingWalk.WalkImageUrl=walk.WalkImageUrl;
            existingWalk.DifficultyId=walk.DifficultyId;
            existingWalk.RegionId=walk.RegionId;

            await dbContext.SaveChangesAsync();
            return existingWalk;
            
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
           var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);
            if(existingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();

            return existingWalk;

        }
    }
}
