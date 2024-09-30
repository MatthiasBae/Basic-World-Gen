
using Godot;

[Tool]
[GlobalClass]
public partial class StrategyTileVariant : Resource {
    [Export]
    public Vector2I AtlasCoordinates;
    [Export]
    public float Probability;
}