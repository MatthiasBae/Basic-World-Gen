
using System.Collections.Generic;
using System.Linq;
using Godot;

[Tool]
[GlobalClass]
public partial class StrategyTile : Resource {
    [Export]
    public int BinaryValue;
    [Export]
    public int TileSetSourceId;
    [Export]
    public Godot.Collections.Array<StrategyTileVariant> Variants;
    [Export]
    public bool IsEdge;
    
    public bool TryAddVariant(StrategyTileVariant variant) {
        if (this.ContainsVariant(variant.AtlasCoordinates)) {
            return false;
        }
        
        this.Variants.Add(variant);
        return true;
    }
    
    public bool ContainsVariant(Vector2I atlasCoordinates) {
        foreach(var variant in this.Variants) {
            if (variant.AtlasCoordinates == atlasCoordinates) {
                return true;
            }
        }
        
        return false;
    }

    public StrategyTileVariant GetVariant(int index) {
        if (this.Variants.Count == 0 || index >= this.Variants.Count) {
            return null;
        }
        
        return this.Variants[index];
    }
    
    public StrategyTileVariant GetRandomVariant() {
        var probability = GD.Randf();
        
        foreach(var variant in this.Variants) {
            if (probability >= variant.Probability) {
                return variant;
            }
        }
        
        return this.Variants[0];
    }
}