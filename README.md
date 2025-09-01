# Final Project: Boulder2

Boulder2 is a simple 3d mobile game that draws inspiration from classics like doodle jump and agar.io, while maintaining an arcade / snythwave aesthetic.

# Made with:

- Unity6.1 (metal)
- No third-party assets.
- Only official system packages from Unity (e.g. the InputManager or URP)
- All art created solely for this project.
- All music customly made for this project (MIDI DAW).
- Custom shaders.

# Gameplay

- Green square: collectible to be consumed by player - 1 point added to score.
- Red cube: appears after a short time randomly around the map. Can be consumed for 0 points. If not consumed within 15s, turns into a "GameOver" triggering barrier.
- Blue shockwave: alerts the player in what direction a red square has spawned.
- Outer walls: contact with the player results in "GameOver".

# Known Issues

- For reasons unknown, the custom unlit fragment shader responsible for animating the shockwave is not rendered on ios (android unknown). It is possible that the main camera does not recognize the material and prefab as being in view as it is being set by `MaterialPropertyBlock` , resulting in it being culled before it is rendered.

# Resources Used

- Unity6
- Ableton 10 Live
- Audacity3
- Affinity Designer2
- Adobe Photoshop
- Shader Cookbook
- Visual Studio / Code
- Various editing plugins
- Google Fonts
