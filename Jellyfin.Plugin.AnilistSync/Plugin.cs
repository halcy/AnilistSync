using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Jellyfin.Plugin.AnilistSync.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.AnilistSync
{

    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public override string Name => "AnilistSync";
        public override Guid Id => Guid.Parse("18c2a8ea-afa0-4a0b-aa94-072b492ab80b");
        public override string Description => "Jellyfin plugin to scrobble to Anilist";
        public Version version = new Version("2.2.0.0");

        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public static Plugin? Instance { get; private set; }


        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = this.Name,
                    EmbeddedResourcePath = string.Format("{0}.Configuration.configPage.html", GetType().Namespace)
                }
            };
        }
    }
}