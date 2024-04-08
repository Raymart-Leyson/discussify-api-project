using DiscussifyApi.Dtos;
using System.Security.Claims;
using DiscussifyApi.Models;

namespace DiscussifyApi.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// This method is used to create a new authentication token for the user.
        /// </summary>
        /// <param name="data">The data of the user.</param>
        /// <param name="type">The type of the user.</param>
        /// <returns>
        /// The authentication result which contains the access token and the refresh token.
        /// </returns>
        Task<AuthenticationResult> CreateAuthToken(string data, string type);

        /// <summary>
        /// This method is used to verify the access token sent in the request header.
        /// </summary>
        /// <param name="accessToken">The access token sent in the request header.</param>
        /// <returns>
        /// The claims principal of the user if the access token is valid.
        /// Otherwise, null.
        /// </returns>
        ClaimsPrincipal VerifyAccessToken(string accessToken);

        /// <summary>
        /// This method is used to verify the refresh token sent in the request header.
        /// </summary>
        /// <param name="refreshToken">The refresh token sent in the request header.</param>
        /// <param name="validData">The data of the user if the refresh token is valid.</param>
        /// <returns>
        /// True if the refresh token is valid.
        /// Otherwise, false.
        /// </returns>
        Task<bool> VerifyRefreshToken(string refreshToken, out string validData);

        /// <summary>
        /// This method is used to generate a new refresh token for the user.
        /// </summary>
        /// <param name="data">The data of the user.</param>
        /// <param name="type">The type of the user.</param>
        /// <returns>
        /// The new refresh token.
        /// </returns>
        string GenerateRefreshToken(string data, string type);
    }
}