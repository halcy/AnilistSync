using System;

namespace Jellyfin.Plugin.AnilistSync.API.Exceptions
{
    /// <inheritdoc />
    public class AnilistAPIException : Exception
    {
        /// <summary>
        /// Anilist error status code
        /// </summary>
        public int? statusCode;

        /// <summary>
        /// Array of locations of errors 
        /// </summary>
        public Location[]? locations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnilistAPIException"/> class.
        /// </summary>
        public AnilistAPIException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnilistAPIException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public AnilistAPIException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnilistAPIException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception.</param>
        public AnilistAPIException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnilistAPIException"/> class.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">The error status code</param>
        /// <param name="locations">The error locations array</param>
        public AnilistAPIException(string? message, int? statusCode, Location[]? locations)
            : base(message)
        {
            this.statusCode = statusCode;
            this.locations = locations;
        }
    }
}
