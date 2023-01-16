**Work in progress!** Not really useful yet.

## Installation

- Make sure [Melon Loader](https://melonwiki.xyz/#/?id=what-is-melonloader) is installed in Ultrakill
- Clone this repository
- Follow instructions in the _Build_ section
- Copy `bin/Debug/UltrakillTas.dll` into `ULTRAKILL/Mods/UltrakillTas.dll`
  - You can usually find your `ULTRAKILL` directory at `C:/Program Files (x86)/Steam/steamapps/common/ULTRAKILL`

## Usage

- Press `P` to pause/unpause the game

## Build

- Make sure a C# compiler is installed
- \[optional] Create `GamePath.txt` in this repository's root which contains a path to the game directory where MelonLoader is installed (so that `.dll` references are resolved)
  - This is not required if the game is in the default location: `C:/Program Files (x86)/Steam/steamapps/common/ULTRAKILL`
- Build by running `msbuild`
