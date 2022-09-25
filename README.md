# Conditional Objects
Unity scripts for conditionally changing object properties or removing objects from Scenes and Prefabs.
All processing is done in the editor at import/build time, so there's no runtime penalty in built players.


## Features
- Supported conditions:
  + Editor vs Built player
  + Development vs Release build settings
  + Current platform
- Conditionally change `GameObject` or `Component` properties with the [ConditionalProperties](Runtime/ConditionalProperties.cs) script, based on the supported conditions above (e.g.: setting different store URLs between Android and iOS)
- Conditionally remove `GameObject`s or `Component`s with the [ConditionalObjects](Runtime/ConditionalObjects.cs) script, based on the supported conditions above (e.g.: setting up a debug object only in development builds)


## How to install
Install via [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
using the following URL:

```
https://github.com/gilzoide/unity-conditional-objects.git
```


## Unity support
`ConditionalObjects` and `ConditionalProperties` in Scenes are supported in all Unity versions. 
Before Unity 2020.2, prefabs are only supported when directly instanced in scenes.