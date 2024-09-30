using Godot;
using System;
using System.Collections.Generic;

public partial class TilemapLayer : TileMapLayer {
    
    public enum Layer {
        Underground = 0,
        SubSurface = 1,
        Water = 2,
        Surface = 3,
    }
    
    public enum LayerCategory {
        Terrain = 0,
        Floor = 1,
    }

    [Export]
    public StrategyTileSet StrategyTileSet;
    
    public static LayerCategory GetLayerCategory(Layer layer) {
        //a dictionary would be more efficient
        return layer switch {
            Layer.Surface => LayerCategory.Terrain,
            Layer.Water => LayerCategory.Terrain,
            Layer.SubSurface => LayerCategory.Terrain,
            Layer.Underground => LayerCategory.Terrain,
            _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, null)
        };
    }
}
