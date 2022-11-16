# Conditional Objects
Scripts that modify `GameObject`s and `Component`s at Prefab/Scene import time, based on build configurations.
All processing is done in the editor at import/build time, so there's no runtime penalty in built players.


## Features
- Supported conditions:
  + Editor vs Built player
  + Development vs Release build settings
  + Scripting define symbols
  + Active build platform
- Conditionally modify `GameObject` or `Component` properties with the [PropertyModifier](Runtime/PropertyModifier.cs) script, based on the supported conditions above (e.g.: setting different store URLs between Android and iOS)
- Conditionally keep/delete `GameObject`s or `Component`s with the [KeepObjectsModifier](Runtime/KeepObjectsModifier.cs)/[DeleteObjectsModifier](Runtime/DeleteObjectsModifier.cs) scripts, based on the supported conditions above (e.g.: setting up a debug object only in development builds)


## How to install
Install via [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
using the following URL:

```
https://github.com/gilzoide/unity-conditional-objects.git
```


## Unity support
`ImportTimeObjectModifier` in Scenes are supported in all Unity versions. 
Before Unity 2020.2, prefabs are only supported when directly instanced in scenes.


## Creating your own modifier
1. Create a script that inherits from [ImportTimeObjectModifier](Runtime/ImportTimeObjectModifier.cs)
2. Implement the abstract `void Apply(bool filtersMatch)` method inside a `#if UNITY_EDITOR` block.
   Check out any of the builtin scripts for an example.
3. Add the script to your Prefabs/Scenes

Whenever a prefab with your script is imported or a scene is processed (played in editor or built), the modifier will be applied and immediately destroyed.