**Work in progress!** Not really useful yet. See the [Future plans](#Future-plans) section below.

## Installation

- Make sure [Melon Loader](https://melonwiki.xyz/#/?id=what-is-melonloader) is installed in Ultrakill
- Clone this repository
- Follow instructions in the _Build_ section
- Copy `bin/Debug/UltrakillTas.dll` into `ULTRAKILL/Mods/UltrakillTas.dll`
  - You can usually find your `ULTRAKILL` directory at: `C:/Program Files (x86)/Steam/steamapps/common/ULTRAKILL`

## Usage

- Press `P` to pause/unpause the game

## Build

- Make sure a C# compiler is installed
- \[optional] Create `GamePath.txt` in this repository's root which contains a path to the game directory where MelonLoader is installed (so that `.dll` references are resolved)
  - This is not required if the game is in the default location: `C:/Program Files (x86)/Steam/steamapps/common/ULTRAKILL`
- Install dependencies with: `nuget restore UnityTas.sln`
- Build by running: `msbuild`

## Test

```sh
msbuild -p:Configuration=Test && csi ./tests/src/RunTests.csx
```

## Future plans

I'm writing this tool in a way that it can be used for any Unity game. To start with I'm building it for Ultrakill (since I'm into that game at the moment) but eventually it should support almost any Unity game. These are my plans for how I think it should work when it is done:

- **Input**
  - The tool will manipulate `Time.timeScale` (among others) to step through the game one frame at a time.
  - Harmony will be used to patch the Unity input methods so that the game does not receive input from the user while it is being controlled by the tool.
  - These patches will also be used to send specific inputs to the game.
  - The tool will display a UI to select inputs, step one frame forward and save/load state.
    - May also have other controls like an option to play the game normally (or in slow motion) for a period of time.
  - Each game will have a specific set of input UI controls displayed based on which controls are used in the game.
- **Game manifest**
  - When the game is launched, the tool will generate a file containing some metadata used to identify the game and options for the tool (eg. control bindings for the game).
  - \[optional] It could use reflection to generate a listing of all possible state and give the user the option of which state it should save or discard.
    - This may be useful for reducing the size of save states or possibly necessary if restoring some state corrupts the game somehow.
  - If the game manifest already exists it will not be regenerated.
  - Example:
    ```yaml
    game:
      name: Name of game
      developer: Name of dev
    state:
      # Every class in the game assembly is listed in here along with their
      # fields (whether public or private) and how the tool should treat them
      SomeClass:
        static:
          # `save` means it is a part of the state which should be included when
          # saving/loading state.
          SomeStaticField: save
        nonstatic:
          # `discard` means it is not part of the state (eg. it's something to do
          # with analytics, the game's save/load mechanism or something else that
          # has no impact on gameplay or operation of the tool).
          _unimportantField: discard
    components:
      # Every Unity component in the game assembly is listed here along with
      # whether the tool should save their state or not.
      SomeComponent: save
    ```
  - When first generated, all state will default to `save`.
  - Only _fields_ (not properties) are listed since they are the things which actually contain the data that properties get and set from.
  - When there are fields missing from the game manifest, this usually indicates an update to the game. The tool can optionally add these fields to the manifest so that the user can update them to their proper values.
- **Determinism**
  - Unity's `Random` methods will be seeded at the start and `Random.state` saved/loaded to provide deterministic random values.
  - Timing methods (eg. `.Invoke(someDelay)`) will be patched and manually triggered on the frame they should trigger or discarded when loading state.
  - Could possibly add the ability to force a specific random value at a certain time/method.
    - I don't know how realistic this is since you could cause some very strange behavior and the specific values chosen may not be possible using the Xorshift 128 algorithm Unity uses to generate random numbers.
  - Note that there may still be sources of non-determinism (eg. asynchronous loading of assets causing a game object to be created at a different time, multithreading).
  - If determinism is not possible for some reason my fallback is to save state every frame (but only the changes to save space).
    - This should force it to have replayability at the cost of more storage and probably not being able to play the game at full speed.
- **Extensibility**
  - The game manifest should allow the tool to work out of the box for most games. However some games may need special considerations.
    - For example a game may use other non-Unity APIs for timing, randomness and randomness (`System.DateTime`, reading state from a file, etc.)
  - The tool will be packaged for use in external C# projects so that others can build their own TAS mods and custom workarounds for their specific game.
  - This will also allow building specialized tools for certain games.
    - For example an aimbot that calculates a position based on state specific to that game.
  - These game-specific TAS mods should include a prefilled game manifest and will work out of the box.
  - UnityTas will still be both a package to be used by other TAS mods and its own standalone mod though. This will allow users without modding experience to TAS any Unity game they want (provided the game doesn't require any special setup or workarounds).
  - I may also decide to keep a central repository of game manifests/assemblies containing specialized tools which the mod can download when being run with a game.
    - This should also allow it to work for game updates without needing to update the TAS mod (still need to wait for the game manifest in the central repository to be updated though).
