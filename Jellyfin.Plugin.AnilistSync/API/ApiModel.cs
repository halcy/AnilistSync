using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.AnilistSync.API
{
    /// <summary>
    /// Media list status enum,
    /// </summary>
    public enum MediaListStatus
    {
        /// <summary>Currently watching.</summary>
        CURRENT,
        /// <summary>Planning to watch.</summary>
        PLANNING,
        /// <summary>Completed.</summary>
        COMPLETED,
        /// <summary>Dropped.</summary>
        DROPPED,
        /// <summary>n hold.</summary>
        PAUSED,
        /// <summary>Rewatching.</summary>
        REPEATING
    }

    /// <summary>
    /// Root JSON object.
    /// </summary>
    public class RootObject
    {
        /// <summary>
        /// Gets or sets data.
        /// </summary>
        [JsonPropertyName("data")]
        public Data? Data { get; set; }

        /// <summary>
        /// Gets or sets errors.
        /// </summary>
        [JsonPropertyName("errors")]
        public AnilistError[]? Errors { get; set; }
    }

    /// <summary>
    /// Data JSON Object.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Gets or sets user.
        /// </summary>
        [JsonPropertyName("Viewer")]
        public User? User { get; set; }

        /// <summary>
        /// Gets or sets list entry.
        /// </summary>
        [JsonPropertyName("SaveMediaListEntry")]
        public ListEntry? ListEntry { get; set; }

        /// <summary>
        /// Gets or sets media.
        /// </summary>
        [JsonPropertyName("Media")]
        public Media? Media { get; set; }
    }

    /// <summary>
    /// Error object.
    /// </summary>
    public class AnilistError
    {
        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets error status.
        /// </summary>
        [JsonPropertyName("status")]
        public int? ErrorStatus { get; set; }

        /// <summary>
        /// Gets or sets error locations.
        /// </summary>
        [JsonPropertyName("locations")]
        public Location[]? Locations { get; set; }
    }

    /// <summary>
    /// Locations of error(s) object.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets or sets line.
        /// </summary>
        [JsonPropertyName("line")]
        public int? Line { get; set; }

        /// <summary>
        /// Gets or sets column.
        /// </summary>
        [JsonPropertyName("column")]
        public int? Column { get; set; }
    }

    /// <summary>
    /// List Entry object.
    /// </summary>
    public class ListEntry
    {
        /// <summary>
        /// Gets or sets mediaId.
        /// </summary>
        [JsonPropertyName("mediaId")]
        public int? MediaId { get; set; }

        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets progress.
        /// </summary>
        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        [JsonPropertyName("status")]
        public MediaListStatus? Status { get; set; }
    }

    /// <summary>
    /// Media object.
    /// </summary>
    public class Media
    {
        /// <summary>
        /// Gets or sets episodes.
        /// </summary>
        [JsonPropertyName("episodes")]
        public int? Episodes { get; set; }
    }

    /// <summary>
    /// User object.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets user ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    /// <summary>
    /// GraphQLBody object.
    /// </summary>
    public class GraphQLBody
    {
        /// <summary>
        /// Gets or sets GraphQL query string.
        /// </summary>
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        /// <summary>
        /// Gets or sets GraphQL variables.
        /// </summary>
        [JsonPropertyName("variables")]
        public ListEntry? Variables { get; set; }
    }

    /// <summary>
    /// OAuth response object.
    /// </summary>
    public class OAuth
    {
        /// <summary>
        /// Gets or sets grant type.
        /// </summary>
        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }

        /// <summary>
        /// Gets or sets client ID.
        /// </summary>
        [JsonPropertyName("client_id")]
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets client secret.
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets redirect URI.
        /// </summary>
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }

    /// <summary>
    /// Code response object.
    /// </summary>
    public class CodeResponse
    {
        /// <summary>
        /// Gets or sets token type.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        /// <summary>
        /// Gets or sets expires in.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets access token.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets refresh token.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
