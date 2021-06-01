using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.AnilistSync.API
{
    public class RootObject
    {
        [JsonPropertyName("data")]
        public Data? Data { get; set; }

        [JsonPropertyName("errors")]
        public Error[]? Errors { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("Viewer")]
        public User? User { get; set; }

        [JsonPropertyName("SaveMediaListEntry")]
        public ListEntry? ListEntry { get; set; }

        [JsonPropertyName("Media")]
        public Media? Media { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("message")]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("status")]
        public int? ErrorStatus { get; set; }

        [JsonPropertyName("locations")]
        public Location[]? Locations { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("line")]
        public int? Line { get; set; }

        [JsonPropertyName("column")]
        public int? column { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("episodes")]
        public int? Episodes { get; set; }
    }

    public class User
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class ListEntry
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        [JsonPropertyName("status")]
        public MediaListStatus? Status { get; set; }
    }

    public enum MediaListStatus
    {
        CURRENT,
        PLANNING,
        COMPLETED,
        DROPPED,
        PAUSED,
        REPEATING
    }

    public class CodeResponse
    {
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

    }



    public class AnilistAPIException : Exception
    {
        public Error[]? errors;

        public AnilistAPIException()
        {

        }

        public AnilistAPIException(string message)
            : base(message)
        {
            
        }

        public AnilistAPIException(string message, Exception inner)
                : base(message, inner)
        {

        }

        public AnilistAPIException(Error[] errors)
        {
            this.errors = errors;
        }
    }
}
