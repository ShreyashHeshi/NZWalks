using NZWalks.API.Models.Domain;
using System.Net;

namespace NZWalks.API.Repositories
{
    public interface IImageRepositary
    {
        Task<Image> Upload(Image image);
    }
}
