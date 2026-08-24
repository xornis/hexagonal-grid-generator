# Hexagonal Constructor
Hexagonal Constructor is a lightweight procedural hex-grid generator for Unity.
It supports both **randomized** generation (BranchWalk, ClusterGrowth) and **shape** generation (Disk, Ring, etc.), and includes a clean custom inspector with live preview and debugging tools.

## Features

### Generation Modes:
- **Randomized generation:** BranchWalk, ClusterGrowth
- **Shape generation:** Disk, Ring, Spiral, Triangle, Rectangle, Rhombus

### Settings Components:
- **Grid Settings** - Prefab, scale, orientation, radius
- **Generation Settings** - Start position, algorithm selection, seed (randomized only)
- **Preview Settings** (optional) - Edit Mode Gizmos visualization, color, scale
- **Debug Settings** (optional) - Step-by-step generation with delay

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

## Support
- For questions, bug reports, or feature requests, contact me: xornis.dev@gmail.com
