# ghibli-clouds-new

Unity plugin files are included under:

- `Assets/GhibliClouds/Runtime/WorleyNoise3D.cs`
- `Assets/GhibliClouds/Runtime/GhibliCloudMetaMesh.cs`

## What it does

- Generates a procedural cloud mesh in Unity.
- Uses 3D Worley noise to shape cloud puff details.
- Blends metaball-style density to create a soft "meta mesh" cloud volume.

## Quick usage

1. Add the `Assets/GhibliClouds` folder to your Unity project (or open this repo as a Unity project).
2. Create a GameObject with a `MeshFilter` and `MeshRenderer`.
3. Add `GhibliClouds.Runtime.GhibliCloudMetaMesh`.
4. Tune:
   - `Noise Frequency` / `Noise Strength` for Worley cloud breakup.
   - `Metaball Count` / `Metaball Radius` / `Metaball Strength` for meta mesh blending.
5. Use the component context menu **Regenerate Cloud Meta Mesh** to rebuild after changes.
