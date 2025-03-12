using Microsoft.EntityFrameworkCore.Update.Internal;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepositary
    {
        Task<List<Region>> GetAllAsync();

        Task<Region?> GetByIdAsync(Guid id);
        // Task has return type of region
        // region can be null or a value so i put ? nullable region

        Task<Region> CreateAsync(Region region);

        Task<Region?> UpdateAsync(Guid id, Region region);

       Task<Region?> DeleteAsync(Guid id);

    }
}
