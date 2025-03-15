using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepositary
    {
        Task<Walk> CreateAsync(Walk walk);

        Task<List<Walk>> GetAllAsync(string? filterOn=null, string? filterQuery=null,
            string? sortBy=null, bool isAscending=true, int pageNumber=1, int pageSize=1000);

        Task<Walk?> GetByIdAsync(Guid id);
        // this will receive single parameter of type Guid and it will return task of type walk

        Task<Walk?> UpdateAsync(Guid id, Walk walk);

        Task<Walk?> DeleteAsync(Guid id);
    }
}
