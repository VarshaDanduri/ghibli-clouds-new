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

## WebGL website setup

This repository now includes a web host page at `docs/index.html` for Unity WebGL output.

1. In Unity Hub, click **Add** and select the repository root folder.
2. Add your cloud scene and include it in **Build Settings**.
3. In Unity, switch platform to **WebGL**.
4. Build the project to `docs/Build`.
5. In **Project Settings > Player > Product Name**, set the product name to `ghibli-clouds`.
6. Ensure your build outputs are named:
   - `ghibli-clouds.loader.js`
   - `ghibli-clouds.data`
   - `ghibli-clouds.framework.js`
   - `ghibli-clouds.wasm`
7. Serve the `docs` folder (or publish it with GitHub Pages) and open `index.html`.
