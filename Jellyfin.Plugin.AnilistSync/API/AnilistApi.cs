using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediaBrowser.Common.Json;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Model.Dto;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.AnilistSync.API
{
    public class AnilistApi
    {
        private readonly ILogger<AnilistApi> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        private const string listIdQuery = @"mutation ($mediaId: Int) {SaveMediaListEntry(mediaId: $mediaId) {id, status, repeat}}&variables={""mediaId"": ""{0}""}";
        private const string listUpdateQuery = @"mutation ($id: Int, $progress: Int, $status: MediaListStatus, $repeat: Int) {SaveMediaListEntry(id: $id, progress: $progress, status: $status, repeat: $repeat) {id, progress, status, repeat}}";
        private const string episodeQuery = @"query ($id: Int) {Media (id: $id) {episodes}}";
        private const string currentUserQuery = @"query {Viewer {id, name}}";

        private const string listUpdateVars1 = @"&variables={""id"":""{0}"", ""progress"":""{1}""}";
        private const string listUpdateVars2 = @"&variables={""id"":""{0}"", ""progress"":""{1}"", ""status"":""{2}""}";
        private const string listUpdateVars3 = @"&variables={""id"":""{0}"", ""progress"":""{1}"", ""status"":""{2}"", ""repeat"":""{3}""}";

        public const string BaseOauthUrl = @"https://anilist.co/api/v2";
        public const string BaseGraphQLUrl = @"https://graphql.anilist.co/api/v2?query=";
        public const string RedirectUri = BaseOauthUrl + @"/oauth/pin";
        public const string ClientId = @"5659";
        public const string Secret = @"h7ym2GZ6OjrdJ9sygDP7kDnQWsBdTwp4U8s7pt4X";

        public AnilistApi(ILogger<AnilistApi> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = JsonDefaults.GetOptions();
        }


        public async Task<CodeResponse?> GetToken(string? code)
        {
            var uri = $"/oauth/token";

            var payload = $"{{\"grant_type\": \"authorization_code\",\"client_id\": {ClientId}, \"client_secret\": \"{Secret}\", \"redirect_uri\": \"{BaseOauthUrl}/oauth/pin\",\"code\": \"{code}\"}}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).PostAsync(BaseOauthUrl + uri, content);
            return await responseMessage.Content.ReadFromJsonAsync<CodeResponse>(_jsonSerializerOptions);
        }

        public async Task<RootObject?> GetUser(string? userToken)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(
                BaseGraphQLUrl +
                currentUserQuery);
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");
            
            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).SendAsync(requestMessage);

            var data = await responseMessage.Content.ReadFromJsonAsync<RootObject>(_jsonSerializerOptions);
            if (data?.Errors != null)
            {
                throw new AnilistAPIException(data.Errors);
            }
            return data;
        }

        public async Task<RootObject?> GetListEntry(string anilistId, string? userToken)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(
                BaseGraphQLUrl + 
                listIdQuery.Replace("{0}", anilistId));
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            requestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).SendAsync(requestMessage);
            var data = await responseMessage.Content.ReadFromJsonAsync<RootObject>(_jsonSerializerOptions);
            if (data?.Errors != null)
            {
                throw new AnilistAPIException(data.Errors);
            }
            return data;
        }

        public async Task<RootObject?> GetEpisodes(string anilistId)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(
                BaseGraphQLUrl + 
                episodeQuery + 
                $"&variables={{\"id\":{anilistId}}}");
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).SendAsync(requestMessage);
            var data = await responseMessage.Content.ReadFromJsonAsync<RootObject>(_jsonSerializerOptions);
            if (data?.Errors != null)
            {
                throw new AnilistAPIException(data.Errors);
            }
            return data;
        }

        public async Task<RootObject?> PostListUpdate(string? anilistMediaId, string? userToken, int? progress, MediaListStatus? status, int? timesRewatched)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(
                BaseGraphQLUrl + 
                listUpdateQuery + 
                listUpdateVars3
                    .Replace("{0}", anilistMediaId)
                    .Replace("{1}", progress.ToString())
                    .Replace("{2}", status.ToString())
                    .Replace("{3}", timesRewatched.ToString()));
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            requestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient(NamedClient.Default).SendAsync(requestMessage);
            var data = await responseMessage.Content.ReadFromJsonAsync<RootObject>(_jsonSerializerOptions);
            if (data?.Errors != null)
            {
                throw new AnilistAPIException(data.Errors);
            }
            return data;
        }
    }
}