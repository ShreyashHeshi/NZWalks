using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepositary
    {
        Task<Walk> CreateAsync(Walk walk);

        Task<List<Walk>> GetAllAsync();

        Task<Walk?> GetByIdAsync(Guid id);
        // this will receive single parameter of type Guid and it will return task of type walk

        Task<Walk?> UpdateAsync(Guid id, Walk walk);

        Task<Walk?> DeleteAsync(Guid id);
    }
}
