using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediaBrowser.Common.Json;
using MediaBrowser.Common.Net;
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.AnilistSync.API.Exceptions;

namespace Jellyfin.Plugin.AnilistSync.API
{
    public class AnilistApi
    {
        private readonly ILogger<AnilistApi> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public const string BaseOauthUrl = @"https://anilist.co/api/v2";
        public const string GraphQLUrl = @"https://graphql.anilist.co";

        public const string QueryString = @"
mutation ($id: Int, $mediaId: Int, $status: MediaListStatus, $progress: Int,) { 
  SaveMediaListEntry(id: $id, mediaId: $mediaId, status: $status, progress: $progress ) { 
    id,
    mediaId,
    status,
    progress
  }
}";
        
        public const int ClientId = 5659;
        public const string Secret = @"h7ym2GZ6OjrdJ9sygDP7kDnQWsBdTwp4U8s7pt4X";

        public AnilistApi(ILogger<AnilistApi> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = JsonDefaults.GetOptions();
        }


        public async Task<CodeResponse?> GetToken(string? code)
        {
            string uri = @"/oauth/token";
            string payload = JsonSerializer.Serialize(new OAuth
            {
                GrantType = "authorization_code",
                ClientId = ClientId,
                ClientSecret = Secret,
                RedirectUri = $"{ BaseOauthUrl }/oauth/pin",
                Code = code
            });
            using HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).PostAsync(BaseOauthUrl + uri, content);
            return await responseMessage.Content.ReadFromJsonAsync<CodeResponse>(_jsonSerializerOptions);
        }

        public async Task<RootObject?> GetEpisodes(int? anilistId)
        {
            GraphQLBody content = new GraphQLBody { 
                Query = @"query ($mediaId: Int) { Media (id: $mediaId) { episodes }}",
                Variables = new ListEntry
                {
                    MediaId = anilistId,
                    Id = null,
                    Progress = null,
                    Status = null
                }
            };
            return await Post(content);
        }

        public async Task<RootObject?> GetUser(string? userToken)
        {
            GraphQLBody content = new GraphQLBody { 
                Query = @"query { Viewer { id, name }}",
                Variables = null
            };
            return await PostWithAuth(userToken, content);
        }

        public async Task<RootObject?> GetListEntry(string? userToken, int? anilistId)
        {
            GraphQLBody content = new GraphQLBody
            {
                Query = QueryString,
                Variables = new ListEntry
                {
                    MediaId = anilistId,
                    Id = null,
                    Progress = null,
                    Status = null
                }
            };
            return await PostWithAuth(userToken, content);
        }

        public async Task<RootObject?> PostListStatusUpdate(string? userToken, int? listId, MediaListStatus? status)
        {
            GraphQLBody content = new GraphQLBody
            {
                Query = QueryString,
                Variables = new ListEntry
                {
                    MediaId = null,
                    Id = listId,
                    Progress = null,
                    Status = status
                }
            };
            return await PostWithAuth(userToken, content);
        }

        public async Task<RootObject?> PostListProgressUpdate(string? userToken, int? listId, int? progress)
        {
            GraphQLBody content = new GraphQLBody
            {
                Query = QueryString,
                Variables = new ListEntry
                {
                    MediaId = null,
                    Id = listId,
                    Progress = progress,
                    Status = null
                }
            };
            return await PostWithAuth(userToken, content);
        }

        public async Task<RootObject?> Post(GraphQLBody graphQL)
        {
            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).PostAsJsonAsync<GraphQLBody>(GraphQLUrl, graphQL, _jsonSerializerOptions);
            var root = await responseMessage.Content.ReadFromJsonAsync<RootObject>(_jsonSerializerOptions);
            if (root?.Errors != null)
            {
                foreach (AnilistError error in root.Errors)
                {
                    throw new AnilistAPIException(error.ErrorMessage, error.ErrorStatus, error.Locations);
                }
            }
            return root;
        }

        public async Task<RootObject?> PostWithAuth(string? userToken, GraphQLBody graphQL)
        {
            using var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(GraphQLUrl),
                Method = HttpMethod.Post
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

            string content = JsonSerializer.Serialize(graphQL, _jsonSerializerOptions);
            requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).SendAsync(requestMessage);
            var root = await responseMessage.Content.ReadFromJsonAsync<RootObject>(_jsonSerializerOptions);
            if (root?.Errors != null)
            {
                foreach (AnilistError error in root.Errors)
                {
                    throw new AnilistAPIException(error.ErrorMessage, error.ErrorStatus, error.Locations);
                }
            }
            return root;
        }
    }
}