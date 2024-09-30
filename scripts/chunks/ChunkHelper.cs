
public class ChunkHelper {
    public static FieldTerrainLayer[,] GetNeighborsFieldTerrainLayer(Field[,] neighbors, TilemapLayer.Layer layer) {
        var layers = new FieldTerrainLayer[3, 3];
        for (var x = 0; x < 3; x++) {
            for (var y = 0; y < 3; y++) {
                if(neighbors[x, y] == null || neighbors[x, y].Terrain == null) {
                    continue;
                }
                
                layers[x, y] = neighbors[x, y].Terrain.Layers[layer];
            }
        }
        
        return layers;
    }
    
    public static TerrainMask ToTerrainMask(Field[,] neighbors, TilemapLayer.Layer layer) {
        var neighborsFieldTerrainLayer = GetNeighborsFieldTerrainLayer(neighbors, layer);
        return StrategyTilingHelper.GetTerrainMask(neighborsFieldTerrainLayer);
    }    
}