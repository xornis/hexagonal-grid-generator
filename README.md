<img width="1600" height="350" alt="Screenshot_7_5" src="https://github.com/user-attachments/assets/89c40752-6fb4-4381-8a6a-787b54a3ec5e" />

## Hexagonal Constructor is a lightweight procedural hex-grid generator for Unity.
It supports both **randomized** generation (BranchWalk, ClusterGrowth) and **shape** generation (Disk, Ring, etc.), and includes a clean custom inspector with live preview and debugging tools.

## Features

### Generation Modes:
- **Randomized generation:**
  1. BranchWalk
  2. ClusterGrowth

#### Examples:
<img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/56909f8e-26a4-4ae7-a1e6-52b3e4107d31" />
<img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/1b1f8556-c581-4883-aaee-da7510c0baea" />

#### Different BranchWalk generations: 
<img width="800" height="450" alt="gen" src="https://github.com/user-attachments/assets/e9c3b1e9-e770-41a8-8877-735a7327635c" />

- **Shape generation:**
  1. Disk & Ring
  3. Spiral
  4. Triangle
  5. Rectangle
  6. Rhombus

#### Examples:
<img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/ed5abf67-e7b2-4fbe-8ecd-a438cc3e7d3a" />
<img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/23ee9add-44b8-4fbc-b9a0-b7a771f3971b" />
<img width="1200" height="1200" alt="image" src="https://github.com/user-attachments/assets/0a8ca9be-8e12-4560-83f8-f482e670e474" />

### Settings Components:
- **Grid Settings** - Prefab, scale, orientation, radius
- **Generation Settings** - Start position, algorithm selection, seed (randomized only)
- **Preview Settings** (optional) - Edit Mode Gizmos visualization, color, scale
- **Debug Settings** (optional) - Step-by-step generation with delay

#### Example in Inspector: 
<img width="1200" height="857" alt="image" src="https://github.com/user-attachments/assets/b868c08f-94f7-42d5-855c-a6e16b27e7f1" />


### Developer-Friendly:
- Clean namespace: "HexagonalConstructor"
- Lazy initialization via "ContextBehaviour"
- Easy to extend with custom algorithms

## What's Included
- 2+6 built-in generators
- 2 example prefabs (Pointy & Flat hexes)
- Complete documentation (PDF)
- Quick Start guide (README.txt)
- Changelog with version history
- Demo scene

## Quick Start
1. Add "GridGenerator" component to any GameObject
2. Click "Add Required Components" if prompted
3. Assign a hex prefab in Grid Settings
4. Choose generation mode (Shapes or Randomized)
5. Choose a generator algorithm on the selected generation mode.
6. Press Play!
- Optional: Click "Add Optional Components" for Edit Mode preview and step-by-step generation
- Optional: Hide Components by clicking the checkbox in "GridGenerator" script 

## Documentation
- Full PDF documentation included with detailed API reference and examples.
