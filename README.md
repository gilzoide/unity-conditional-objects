# Conditional Objects
Scripts that modify `GameObject`s and `Component`s at Prefab/Scene import time, based on build configurations.
All processing is done in the editor at import/build time, so there's no runtime penalty in built players.


## Features
- Supported conditions:
  + Editor vs Built player
  + Development vs Release build settings
  + Scripting define symbols
  + Active build platform
- Conditionally modify `GameObject` or `Component` properties with the [PropertyModifier](Runtime/PropertyModifier.cs) script (e.g.: setting different controller sprites between PlayStation and Xbox platforms)
- Conditionally keep `GameObject`s or `Component`s with the [KeepObjectsModifier](Runtime/KeepObjectsModifier.cs) script (e.g.: keep a "Login with Google Play Games" button in Android only)
- Conditionally delete `GameObject`s or `Component`s with the [DeleteObjectsModifier](Runtime/DeleteObjectsModifier.cs) script (e.g.: delete a debug button in release builds)


## How to install
Install via [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
using the following URL:

```
https://github.com/gilzoide/unity-conditional-objects.git#1.0.0-preview2
```


## Unity support
`AImportTimeObjectModifier` in Scenes are supported in all Unity versions.

Before Unity 2020.2, prefabs are only supported when directly instanced in scenes.


## Creating your own modifier
1. Create a script that inherits from [AImportTimeObjectModifier](Runtime/AImportTimeObjectModifier.cs)
2. Implement the abstract `void Apply(bool filtersMatch)` method inside a `#if UNITY_EDITOR` block.
   Check out the [DeleteObjectsModifier](Runtime/DeleteObjectsModifier.cs) script for an example.
3. Add the script to your game objects in Prefabs/Scenes

Whenever a prefab with your script is reimported or a scene is processed (either played in editor or built), the modifier will be applied and immediately destroyed.


## Known issues
- `UNITY_EDITOR` and `DEVELOPMENT_BUILD` scripting define symbols are not detected correctly during builds, use the dedicated Editor and Development filters instead
- Extra scripting define symbols passed to `BuildPipeline.BuildPlayer` are not detected correctly during builds
