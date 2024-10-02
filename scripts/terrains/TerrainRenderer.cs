
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TerrainRenderer : Node {

    public static List<FieldTerrainRenderData> GetChunkRenderData(Chunk chunk) {
        var renderData = new List<FieldTerrainRenderData>();
        for(var x = 0; x < chunk.Fields.GetLength(0); x++) {
            for(var y = 0; y < chunk.Fields.GetLength(1); y++) {
                var field = chunk.Fields[x, y];

                renderData.AddRange(GetFieldRenderData(field, Tilemap.Instance));
            }
        }
        
        return renderData;
    } 
    
    public static List<FieldTerrainRenderData> GetFieldRenderData(Field field, Tilemap tilemap) {
        var renderData = new List<FieldTerrainRenderData>();
        var fieldNeighbors = ChunkStore.GetSurroundingFields(field.FieldPosition);
        foreach(var (layer, fieldTerrainLayer) in field.Terrain.Layers.OrderByDescending(entry => entry.Key)) {
            if (!fieldTerrainLayer.HasTerrain) {
                continue;
            }
            var tileSetSourceId = fieldTerrainLayer.Config.AtlasSourceId;
            var terrainMask = ChunkHelper.ToTerrainMask(fieldNeighbors, layer);
            var presenceMask = terrainMask.ToPresenceMask();
            var strategyTileSet = tilemap.Layers[layer].StrategyTileSet;
            var strategyTileSetSource = strategyTileSet.GetTileSetSource(tileSetSourceId);
            var layerBitMask = strategyTileSetSource.BitMask;
            var layerBinaryValue = StrategyTilingHelper.GetBinaryValue(layerBitMask, presenceMask);
            var strategyTile = strategyTileSetSource.GetTile(layerBinaryValue);
            var strategyTileVariant = strategyTile.GetRandomVariant();
                    
            renderData.Add(new FieldTerrainRenderData {
                FieldPosition = field.FieldPosition,
                LocalPosition = field.LocalPosition,
                Layer = layer,
                TileSetSourceId = tileSetSourceId,
                AtlasCoordinate = strategyTileVariant.AtlasCoordinates
            });
                    
            if (!strategyTile.IsEdge) {
                break;
            }
        }
            
        return renderData;
        
    }
    
}