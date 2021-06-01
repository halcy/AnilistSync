using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.AnilistSync.API
{
    [ApiController]
    [Authorize(Policy = "DefaultAuthorization")]
    [Route("AnilistSync")]
    public class Endpoints : ControllerBase
    {
        private readonly AnilistApi _anilistApi;

        public Endpoints(AnilistApi anilistApi)
        {
            _anilistApi = anilistApi;
        }

        [HttpGet("oauth/token/{userCode}")]
        public async Task<ActionResult<CodeResponse?>> GetToken([FromRoute] string userCode)
        {
            return await _anilistApi.GetToken(userCode);
        }

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