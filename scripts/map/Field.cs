
using Godot;

public class Field {
    public Vector2I FieldPosition;
    public Vector2I LocalPosition;
    
    /// <summary>
    /// A 3x3 grid of fields surrounding this field. The center is this field.
    /// </summary>
    // public Field[,] Neighbors;
    public Chunk InChunk;
    
    public FieldBiome Biome;
    public FieldTerrain Terrain;
    
    // public FieldTerrainLayer[,] GetNeighborsFieldTerrainLayer(TilemapLayer.Layer layer) {
    //     var layers = new FieldTerrainLayer[3, 3];
    //     for (var x = 0; x < 3; x++) {
    //         for (var y = 0; y < 3; y++) {
    //             layers[x, y] = this.Neighbors[x, y].Terrain.Layers[layer];
    //         }
    //     }
    //     
    //     return layers;
    // }
    //
    // public TerrainMask ToTerrainMask(TilemapLayer.Layer layer) {
    //     var neighborsFieldTerrainLayer = this.GetNeighborsFieldTerrainLayer(layer);
    //     return StrategyTilingHelper.GetTerrainMask(neighborsFieldTerrainLayer);
    // }
}