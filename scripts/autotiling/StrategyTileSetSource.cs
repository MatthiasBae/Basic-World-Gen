
using System.Collections.Generic;
using System.Linq;
using Godot;

[Tool]
[GlobalClass]
public partial class StrategyTileSetSource : Resource {
    private TileSetAtlasSource _TileSetAtlasSource;
    [Export]
    public int TileSetSourceId;
    private BitMask _BitMask;
    [Export]
    public BitMask BitMask {
        get => this._BitMask;
        set {
            this._BitMask = value;
            if (value != null && this._TileSetAtlasSource != null) {
                this._Initialize(this._TileSetAtlasSource);
            }
            else {
                this.StrategyTiles = new Godot.Collections.Dictionary<int, StrategyTile>();
                this.NonEdgeTileBinaries = new Godot.Collections.Array<int>();
            }
        }
    }
    [Export]
    public Godot.Collections.Array<int> NonEdgeTileBinaries;
    [Export]
    public Godot.Collections.Dictionary<int, StrategyTile> StrategyTiles;
    
    public static StrategyTileSetSource Create(TileSetAtlasSource tileSetAtlasSource, int tileSetSourceId, BitMask bitMask = null) {
        var strategyTileSetSource = new StrategyTileSetSource();
        strategyTileSetSource._TileSetAtlasSource = tileSetAtlasSource;
        strategyTileSetSource.TileSetSourceId = tileSetSourceId;
        
        if (bitMask == null) {
            return strategyTileSetSource;
        }

        strategyTileSetSource.BitMask = bitMask;
        return strategyTileSetSource;
    }

    private void _Initialize(TileSetAtlasSource tileSetAtlasSource) {
        this.StrategyTiles = new Godot.Collections.Dictionary<int, StrategyTile>();
        for(var index = 0; index < tileSetAtlasSource.GetTilesCount(); index++) {
            var atlasCoordinate = tileSetAtlasSource.GetTileId(index);
            
            var tileData = tileSetAtlasSource.GetTileData(atlasCoordinate, 0);
            
            if (tileData == null) {
                continue;
            }
            
            var peeringBitMask = tileData.GetTerrainPeeringBitMask();
            var presenceMask = peeringBitMask.ToPresenceMask();
            var binaryValue = StrategyTilingHelper.GetBinaryValue(this.BitMask, presenceMask);
            
            var strategyTileVariant = new StrategyTileVariant {
                AtlasCoordinates = atlasCoordinate,
                Probability = tileData.Probability
            };

            if(this.StrategyTiles.TryGetValue(binaryValue, out var strategyTile)) {
                strategyTile.TryAddVariant(strategyTileVariant);
                strategyTile.Variants = new Godot.Collections.Array<StrategyTileVariant>(strategyTile.Variants.OrderByDescending(variant => variant.Probability));
                continue;
            }

            strategyTile = new StrategyTile {
                BinaryValue = binaryValue,
                TileSetSourceId = this.TileSetSourceId,
                Variants = new Godot.Collections.Array<StrategyTileVariant> { strategyTileVariant },
                IsEdge = StrategyTilingHelper.IsEdgeTile(tileData)
            };

            //sort descending variants by probability
            
            this.StrategyTiles.Add(binaryValue, strategyTile);
        }
    }
    
    public StrategyTile GetTile(int binaryValue) {
        return this.StrategyTiles.GetValueOrDefault(binaryValue);
    }
    
    public StrategyTileVariant GetRandomTileVariant(int binaryValue) {
        var probability = GD.Randf();
        
        var strategyTile = this.StrategyTiles.GetValueOrDefault(binaryValue);
        if (strategyTile == null) {
            return null;
        }
        
        var variants = strategyTile.Variants;
        foreach(var variant in variants) {
            if (probability >= variant.Probability) {
                return variant;
            }
        }
        
        return variants[0];
    }
    
    public StrategyTileVariant GetTileVariant(int binaryValue) {
        var strategyTile = this.StrategyTiles.GetValueOrDefault(binaryValue);
        if (strategyTile == null) {
            return null;
        }
        
        var variants = strategyTile.Variants;
        return variants[0];
    }
}