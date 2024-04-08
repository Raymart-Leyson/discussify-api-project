using DiscussifyApi.Dtos;

namespace DiscussifyApi.Services
{
    public interface IAnonymousService
    {
        /// <summary>
        /// Creates a new anonymous
        /// </summary>
        /// <param name="anonymous">Anonymous details</param>
        /// <returns>Returns the newly created anonymous</returns>
        Task<AnonymousDto> CreateAnonymous(AnonymousCreationDto anonymous);

        /// <summary>
        /// Gets an anonymous by id
        /// </summary>
        /// <param name="id">Anonymous id</param>
        /// <returns>Returns an anonymous</returns>
        Task<AnonymousDto> GetAnonymousById(int id);
    }
}