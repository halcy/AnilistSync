<h1 align="center">AnilistSync</h1>
<h3 align="center">Big thanks to Crobibero for writing the <a href="https://github.com/crobibero/jellyfin-plugin-simkl">Simkl Plugin</a></h3>

A Jellyfin plugin that scrobbles your anime watches to Anilist for easy automatic list updates.

## Features
- Multi-user support
- Scrobbling of anime shows/movies to Anilist
- Rewatch support


## Requirements
- [Anilist Plugin](https://github.com/jellyfin/jellyfin-plugin-anilist): This plugin relies entirely on the Anilist plugin obtaining the correct ID! If its wrong or not present, scrobbling with fail

## Install
### Repository
1. Add the repository URL to Jellyfin: `https://raw.githubusercontent.com/ARufenach/AnilistSync/master/manifest.json`
2. Install AnilistSync from the plugin catalog
### Github Release
1. Download the `.zip` release from [here](https://github.com/ARufenach/AnilistSync/releases/latest)
2. Extract the contents into a folder in the `plugins` folder of your Jellyfin install directory
