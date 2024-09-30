# World Generation in Godot / C#

This is a small project to show how a open world 2D game can be made with random and customizable terrain generation.

It includes:
- Chunk System which loads and unloads the map in chunks
- Custom Autotiling (only Minimal 3x3 and Full 3x3 possible atm)
- Terrain rendering rules to avoid perfomance issues
- Customizable worlds with configurable biomes and terrains
  

## Chunk System

The main class which controls the chunks is the ChunkController.cs. It distributes each chunk to a different state depending on its distance to the player. 
The chunk controller makes sure that each chunk goes through each state to prepare it for the next possible state.

Depending on your needs you can change the radius of each chunk state area. 

**ChunkController Inspector (Example)**

<img width="382" alt="image" src="https://github.com/user-attachments/assets/60515f48-bf06-476b-a91e-293c6f2ec5c7">

Each chunk state has got 2 Processors. For example state "Loaded" is assigned to processor "Unrender" and "Load". The difference is that one processor is for upgrading the chunk and the other is responsible for downgrading it.

You can apply some settings in the inspector of the processor nodes to adapt to performance issues or lag spikes. Batch Size is the size of chunks processed per frame.

**Example of Inspector**

<img width="392" alt="image" src="https://github.com/user-attachments/assets/aa1ad081-53b0-44b9-a986-dbd83e668507">


## Custom Autotiling

As the set_cells_terrain_connect method in Godot is not optimized for running during runtime I had to write my own custom autotiling.
I used bitmasks to find out what tile I need at which position. For a better explanation how bitmasks work check this link: https://code.tutsplus.com/how-to-use-tile-bitmasking-to-auto-tile-your-level-layouts--cms-25673t

**Only working minimal 3x3 and full 3x3 atm**

All classes belonging to the AutoTiling system are "StrategyTile... .cs" classes.

It is configured in each TileMapLayer Node

<img width="397" alt="image" src="https://github.com/user-attachments/assets/20ab7de4-bdfa-45c6-82d2-9419e69b0c05">

As you can see at the top there is the StrategyTileSet. It is the counterpart to Godots "TileSet". Also "TileSetAtlasSource" and "TileData" have their counterparts in my autotiling system.
The StrategyTileset is a TOOL Script which automatically reads the given TileSet atlas sources and tile data and creates the needed resources for the autotiling system.

If you want to have a minimal 3x3 autotiling you need the 8Bit bitmask, for a full 3x3 autotiling the 5Bit bitmask. Just exchange it in the StrategyTileSet. It will automatically update every tile.
Be aware that you still have to paint the terrains in the TileSet Editor in Godot to work correctly.


## Customizable worlds / biomes

This is the most complicated part. 

### Custom World

Each world can have (in theory) an infinite number of biomes. You can set up worlds using the WorldConfig.cs and exchange it in the "World" Node.

<img width="529" alt="image" src="https://github.com/user-attachments/assets/ae333724-8b37-4b84-b23e-38874fc7e8e3">

You can use the WorldConfig.cs to configure the amount of biomes and the location of those. 

#### Number of biomes

Just add a new BiomeConfig to the BiomeConfig Array in the WorldConfig.cs

#### Location of each biome

The location is defined by 3 Noises. Each noise is representive for a abiotic factor (temperature, humidity and elevation). 
To define the location you have to define the abiotic factor VALUES (not noises, but you can also exchange noises).
Each biome also has got a reference to its own abiotic factor values.

In simple: The algorithm now subtracts VALUES from the Noise Value and checks which biome has got the smallest difference. 

In complex: It is the kNN (k-Nearest-Neighbor) algorithm.
<img width="1237" alt="image" src="https://github.com/user-attachments/assets/c984dc4b-73db-4bad-a0dc-9c10c17c1e09">






