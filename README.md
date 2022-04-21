<h1 align="center">AnilistSync</h1>
<h3 align="center">Big thanks to Crobibero for writing the <a href="https://github.com/crobibero/jellyfin-plugin-simkl">Simkl Plugin</a></h3>

A Jellyfin plugin that scrobbles your anime watches to Anilist for easy automatic list updates.

## Features
- Multi-user support
- Scrobbling of anime shows/movies to Anilist
- Rewatch support


## Requirements
- [Anilist Plugin](https://github.com/jellyfin/jellyfin-plugin-anilist): This plugin relies entirely on the Anilist plugin obtaining the correct ID (**Make sure the metadata of the show contains the correct ID from Anilist!**)! 
- If its wrong or not present, scrobbling with fail

## Install
### Github Release
1. Download the `.zip` release from [here](https://github.com/Fallenbagel/AnilistSync/releases/latest)
2. Extract the contents into a folder in the `plugins` folder of your Jellyfin install directory
3. Install Anilist plugin and activate the metadata provider in the libraries (and ensure it is below your actual metadata provider/low in hierarchy or your metadata will get replaced)
4. Refresh metadata with **Search for missing metadata** to add in anilist ID or manually add it in using the metadata manager (because if this ID is missing this scrobbler will not work!
5. Setup Ani-Sync by authenticating and configure it to your liking!
