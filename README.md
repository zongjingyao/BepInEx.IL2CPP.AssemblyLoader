# AssemblyLoader for BepInEx 6 (IL2CPP)

This is a [BepInEx 6](https://github.com/BepInEx/BepInEx) plugin that allows you to load and run C# assembly file without reopening game.  
It's initially designed for debugging an release-build game that made with Unity IL2CPP.

This plugin is inspired by [BepInEx.ScriptLoader](https://github.com/ghorsington/BepInEx.ScriptLoader).

## Usage

### Install BepInEx

Follow the [install guide](https://docs.bepinex.dev/v6.0.0-pre.1/articles/user_guide/installation/index.html) to install BepInEx 6 for IL2CPP Unity.      
Open game and let BepInEx do initialization steps.

### Install plugin

Clone this repository to a proper location and open it in your favourite IDE or editor.    
Copy dependencies from `BepInEx/interop` and `BepInEx/core` into `Lib` folder.  
Build [AssemblyLoader](./AssemblyLoader) project and copy `AssemblyLoader.dll` into `BepInEx/plugin` folder.

### Write and run assembly

Write your code in [Payload.cs](./Payload/Payload.cs).  
Build [Payload](./Payload) project and copy `Payload.dll` into `BepInEx/assembly` folder. Create this folder if it does not exist. The plugin will automatically load and run `static void Main()` method.

AssemblyLoader can detect `Payload.dll` file changes and reload it automatically.  
You have to do some clean up steps in `static void Unload()` to make your assembly reloadable.

## Others

### Why ManualLogSource instead of UnityEngine.Debug

I always get `Fatal error. System.AccessViolationException: Attempted to read or write protected memory. This is often an indication that other memory is corrupt.` exception when using `UnityEngine.Debug`. I have no idea why it happens, so I have to inject ManualLogSource as a workaround.

### I don't want to copy assemblies every time

You can use `PostBuildEvent` to copy assemblies automatically. Uncomment them in `*.csproj` files and edit `Your destination directory`. 

### Why not compile scripts programmatically

I prefer to use an IDE to write scripts and IDE can cover compilation for me. 
