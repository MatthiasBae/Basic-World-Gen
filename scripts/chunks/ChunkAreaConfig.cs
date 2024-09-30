using Godot;
using System;

[GlobalClass]
public partial class ChunkAreaConfig : Resource {
    [Export]
    public ChunkProcessorBase.ChunkProcessState ProcessState;

    [Export]
    public int Radius;
}
