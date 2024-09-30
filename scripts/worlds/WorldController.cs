
using Godot;

public partial class WorldController : Node {
    public static WorldController Instance;
    
    [Export]
    public World ActiveWorld;
    
    public override void _EnterTree() {
        Instance = this;
    }
}