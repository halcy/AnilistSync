using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.AnilistSync.API
{
    /// <summary>
    /// Anilist endpoints.
    /// </summary>
    [ApiController]
    [Authorize(Policy = "DefaultAuthorization")]
    [Route("AnilistSync")]
    public class Endpoints : ControllerBase
    {
        private readonly AnilistApi _anilistApi;

        /// <summary>
        /// Initializes a new instacne fo the <see cref="Endpoints"/> class.
        /// </summary>
        /// <param name="anilistApi"></param>
        public Endpoints(AnilistApi anilistApi)
        {
            _anilistApi = anilistApi;
        }

        /// <summary>
        /// Gets the OAuth token.
        /// </summary>
        /// <param name="userCode">User code.</param>
        /// <returns>Code response containing token.</returns>
        [HttpGet("oauth/token/{userCode}")]
        public async Task<ActionResult<CodeResponse?>> GetToken([FromRoute] string userCode)
        {
            return await _anilistApi.GetToken(userCode);
        }

        /// <summary>
        /// Gets the currently authenticated user
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Root object containing User object</returns>
        [HttpGet("users/settings/{userId}")]
        public async Task<ActionResult<RootObject?>> GetUser([FromRoute] Guid userId)
        {
            var userConfiguration = Plugin.Instance?.Configuration.GetByGuid(userId);
            if (userConfiguration == null)
            {
                return NotFound();
            }
            return await _anilistApi.GetUser(userConfiguration.UserToken);
        }

    }
}