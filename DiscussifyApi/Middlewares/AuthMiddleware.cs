using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DiscussifyApi.Services;

namespace DiscussifyApi.Middlewares
{
    /// <summary>
    /// This middleware is used to verify the access token sent in the request header.
    /// If the access token is valid, the user is authenticated and the request is passed to the next middleware.
    /// otherwise forbidden access.
    /// </summary>
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// This method is called when the middleware is invoked.
        /// </summary>
        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var user = tokenService.VerifyAccessToken(token);

                if (user != null)
                {
                    context.User = user;
                }
                else 
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Forbidden access");
                    return;
                }
            }

            await _next(context);
        }
    }
}