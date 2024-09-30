using Godot;
using System;
using System.Collections.Generic;

[Tool]
[GlobalClass]
public partial class StrategyTileSet : Resource {
    private TileSet _TileSet;
    [Export]
    public TileSet TileSet {
        get => this._TileSet;
        set {
            this._TileSet = value;
            if (value != null) {
                this._Initialize(value);    
            }
            else {
                this.StrategyTileSetSources = new Godot.Collections.Array<StrategyTileSetSource>();
            }
        }
    }
    
    [Export]
    public BitMask FallbackBitMask;
    
    [Export]
    public Godot.Collections.Array<StrategyTileSetSource> StrategyTileSetSources;

    public static StrategyTileSet Create(TileSet tileSet) {
        var strategyTileSet = new StrategyTileSet();
        strategyTileSet._Initialize(tileSet);
        return strategyTileSet;
    }
    
    private void _Initialize(TileSet tileSet) {
        this.StrategyTileSetSources = new Godot.Collections.Array<StrategyTileSetSource>();
        for(var index = 0; index < tileSet.GetSourceCount(); index++) { 
            var tileSetSource = (TileSetAtlasSource)tileSet.GetSource(index);
            var tileSetSourceId = tileSet.GetSourceId(index);
            
            if(tileSetSource == null) {
                continue;
            }
            
            var strategyTileSetSource = StrategyTileSetSource.Create(tileSetSource, tileSetSourceId, this.FallbackBitMask);
            this.StrategyTileSetSources.Add(strategyTileSetSource);
        }
    }
    
    public StrategyTileSetSource GetTileSetSource(int tileSetSourceId) {
        foreach (var strategyTileSetSource in this.StrategyTileSetSources) {
            if (strategyTileSetSource.TileSetSourceId == tileSetSourceId) {
                return strategyTileSetSource;
            }
        }
        
        GD.PrintErr($"StrategyTiling: Could not find StrategyTileSetSource with id {tileSetSourceId}");
        return null;
    }
    
    public StrategyTileVariant GetTileVariant(int tileSetSourceId, int binaryValue, bool randomTile = false) {
        var strategyTileSetSource = this.GetTileSetSource(tileSetSourceId);
        if (strategyTileSetSource == null) {
            return null;
        }
        
        return randomTile 
            ? strategyTileSetSource.GetRandomTileVariant(binaryValue) 
            : strategyTileSetSource.GetTileVariant(binaryValue);
    }
    
    public Vector2I GetAtlasCoordinate(int tileSetSourceId, int binaryValue, bool randomTile = false) {
        var strategyTileSetSource = this.GetTileSetSource(tileSetSourceId);
        if (strategyTileSetSource == null) {
            return Vector2I.Zero;
        }
        
        var strategyTile = randomTile 
            ? strategyTileSetSource.GetRandomTileVariant(binaryValue) 
            : strategyTileSetSource.GetTileVariant(binaryValue);
        
        return strategyTile.AtlasCoordinates;
    }
}
