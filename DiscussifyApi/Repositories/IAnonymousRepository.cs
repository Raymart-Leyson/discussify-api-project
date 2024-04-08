using DiscussifyApi.Models;

namespace DiscussifyApi.Repositories
{
    public interface IAnonymousRepository
    {
        /// <summary>
        /// Creates a new anonymous
        /// </summary>
        /// <param name="anonymous">Anonymous details</param>
        /// <returns>Returns the id of the newly created anonymous</returns>
        Task<int> CreateAnonymous(Anonymous anonymous);

        /// <summary>
        /// Gets an anonymous by id
        /// </summary>
        /// <param name="id">Anonymous id</param>
        /// <returns>Returns an anonymous</returns>
        Task<Anonymous> GetAnonymousById(int id);
    }
}