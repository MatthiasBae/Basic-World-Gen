using Godot;
using System;

public partial class WorldAreaConfig : Resource {
    [Export]
    public bool IsStaticScene;
    
    [Export(PropertyHint.File, "*.tscn")]
    public PackedScene AreaScene;
}
