using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Tilemap : Node2D {
    public static Tilemap Instance;
    
    [Export(PropertyHint.Enum)]
    public Godot.Collections.Dictionary<TilemapLayer.LayerCategory, TilemapLayerMapping> LayerMapping;
    
    public Dictionary<TilemapLayer.Layer, TilemapLayer> Layers = new();

    public override void _EnterTree() {
        Instance = this;
        this._InitializeLayers();
    }

    private void _InitializeLayers() {
        var children = this.GetChildren();
        
        for(var i = 0; i < children.Count; i++) {
            if (children[i] is not TilemapLayer layer) {
                continue;
            }
            
            this.Layers.Add((TilemapLayer.Layer)i, layer);
        }
    }
    
    public TilemapLayer.Layer GetTopLayer(TilemapLayer.LayerCategory layerCategory) {
        var layerCategoryMapping = this.LayerMapping[layerCategory];
        
        return layerCategoryMapping.Layers.Max();
    }
    
    public TilemapLayer.Layer GetBottomLayer(TilemapLayer.LayerCategory layerCategory) {
        var layerCategoryMapping = this.LayerMapping[layerCategory];
        
        return layerCategoryMapping.Layers.Min();
    }
    
    public TilemapLayer.Layer GetLayerUnderneath(TilemapLayer.Layer layer) {
        return layer - 1;
    }
    
    public TilemapLayer.Layer GetLayerAbove(TilemapLayer.Layer layer) {
        return layer + 1;
    }
    
    public void SetTile(Vector2I fieldPosition, TilemapLayer.Layer layer, int tileSetSourceId, Vector2I? atlasCoordinate = null) {
        this.Layers[layer].SetCell(fieldPosition, tileSetSourceId, atlasCoordinate);
    }
    
    public void ClearTile(Vector2I fieldPosition, TilemapLayer.Layer layer) {
        this.Layers[layer].EraseCell(fieldPosition);
    }
    
    public void ClearTiles(List<Vector2I> fieldPositions, TilemapLayer.Layer layer) {
        foreach(var fieldPosition in fieldPositions) {
            this.Layers[layer].EraseCell(fieldPosition);
        }
    }
}
