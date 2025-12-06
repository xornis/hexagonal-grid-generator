======================================
HEXAGONAL CONSTRUCTOR - QUICK START GUIDE
======================================

Thank you for downloading Hexagonal Constructor!
This package provides procedural hex-grid generation for Unity in two modes:
* Shapes (Disk, Ring, etc.)
* Randomized (deterministic or random)

----------------------------------------
1. Getting Started
----------------------------------------

1) Drag the 'HexRoomGenerator.cs' component onto any GameObject.
2) Assign a hex prefab in the "Tile Visuals" section.
   (Use HexFlat or HexPointy from the "Prefabs" folder, or your own sprite.)
3) Choose Generation Mode (Shapes / Randomized).
4) Press:
	* "Rebuild Preview" (Editor Preview Section) to see the generation preview.
	* or "Rebuild Generation" (Generator Debug Section) to see real generated GameObjects.
	* or turn on Debug Mode to see the generation building one-by-one in Play Mode.

The generator will generate hex tiles as children of the GameObject.

----------------------------------------
2. Inspector Parameters
----------------------------------------

GRID SETTINGS
	TILE VISUALS
		* Hex Prefab - prefab containing SpriteRenderer
		* Hex Scale - visual scale applied to spawned prefabs, scale of the prefab

	TILE GEOMETRY
		* Hex Orientation - FlatTop or PointyTop (affects grid math, NOT sprite rotation)
		* Hex Radius - hex radius used in hex-to-world conversion

GENERATION SETTINGS
	* Mode - choose generation mode
	* Start Axial - axial coordinates of the starting hex in the generator
	
	RANDOM GENERATION
		* Random Algorithm - algorithms of random generations
		* Room Count - number of distinct rooms to generate
		* Use Seed - toggles deterministic generation
		* Seed - numeric seed for reproducible layouts
		* Randomize - creates a pseudo-random numeric seed
	
	SHAPE GENERATION
		* Shape - Disk, Ring, etc.
		* Shape Radius - size of the shape

----------------------------------------
3. Editor Tools
----------------------------------------

EDITOR PREVIEW
	* Enable Preview - toggles this section and preview
	* Rebuild Preview - preview layout before generating
	* Clear Preview - clears cached preview

GENERATOR DEBUG
	* Debug Mode - toggles this section
	* Step Delay - delay of generating hexes one-by-one
	* Rebuild Generation - creates hex tiles in the Scene
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
	* Check Hex Radius and Hex Scale parameters.
If PointyTop looks "flat" or FlatTop looks "pointy":
	* Rotate your sprite by 90 degrees.

----------------------------------------
=== Enjoy building hex-based levels! ===
----------------------------------------
=== Also glad to hear your feedback! ===
----------------------------------------