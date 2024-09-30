using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class FieldTerrain {
    public Dictionary<TilemapLayer.Layer, FieldTerrainLayer> Layers;
    
    public TilemapLayer.Layer GetLayerAbove(TilemapLayer.Layer layer) {
        foreach(var terrainLayer in this.Layers.Keys.OrderBy(entry => entry)) {
            if(terrainLayer > layer) {
                return terrainLayer;
            }
        }
        
        return layer;
    }
    
    public bool HasLayer(TilemapLayer.Layer layer) {
        return this.Layers.ContainsKey(layer);
    }
}
