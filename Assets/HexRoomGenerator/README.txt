HEX ROOM GENERATOR - QUICK START GUIDE
======================================

Thank you for downloading Hex Room Generator!
This package provides procedural hex-grid generation for Unity in two modes:
* Shapes (Disk, Ring, Corridor, etc.)
* Randomized (deterministic or random)

----------------------------------------
1. Getting Started
----------------------------------------
1) Drag the 'HexRoomGenerator.cs' component onto any GameObject.
2) Assign a hex prefab in the "Visual" section.
   (Use HexFlat or HexPointy from the Prefab folder, or your own sprite.)
3) Choose Generation Mode (Shapes / Randomized).
4) Press:
	* "Build/Rebuild Preview" (Preview Section) to see the generation preview.
	* or "Build/Rebuild Generation" (Debug section) to see real generated GameObjects.
	* or turn on Debug Mode to see the generation building one-by-one in Play Mode.

The generator will generate hex tiles as children of the GameObject.

----------------------------------------
2. Inspector Parameters
----------------------------------------

GENERAL
* Orientation - FlatTop or PointyTop (affects grid math, NOT sprite rotation)

VISUAL
* Hex Prefab - prefab containing SpriteRenderer
* Hex Scale - visual scale applied to spawned prefabs, scale of the prefab

GEOMETRY
* Hex Size - hex radius used in hex-to-world conversion

GENERATION
* Mode - choose generation mode

RANDOMIZED
* Random Type - currently supports RandomWalk
* Rooms - number of distinct rooms to generate
* Use Seed - toggles deterministic generation
* Seed - numeric seed for reproducible layouts
* Randomize - creates a pseudo-random numeric seed

SHAPES
* Shape Type - Disk, Ring, Two Rooms + Corridor, etc.
* Radius - size of the shape
* Corridor Thickness - width for corridor-type shapes

----------------------------------------
3. Editor Tools
----------------------------------------

GENERATION PREVIEW
* Preview In Editor - toggles this section and preview
* Build/Rebuild Preview - preview layout before generating
* Clear Preview - clears cached preview

GENERATION DEBUG
* Debug Mode - toggles this section
* Hex Generation Delay - delay of generating hexes one-by-one
* Build/Rebuild Generation - creates hex tiles in the Scene
* Clear Generation - removes generated tiles

----------------------------------------
4. Notes
----------------------------------------

* Hex prefabs are NOT rotated automatically. Rotate your sprite manually
  if you use PointyTop orientation and vice versa. Orientation parameter should match with prefab/sprite orientation.
* Hex generation math is independent of prefab visuals.
* Supports Unity 2021.3 or newer.

----------------------------------------
5. Troubleshooting
----------------------------------------

If tiles overlap:
-> Check Hex Size and Hex Scale parameters.
If PointyTop looks "flat" or FlatTop looks "pointy":
-> Rotate your sprite by 90 degrees.

----------------------------------------
=== Enjoy building hex-based levels! ===
----------------------------------------
=== Also glad to hear your feedback! ===
----------------------------------------