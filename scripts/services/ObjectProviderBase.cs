
using Godot;

public abstract partial class ObjectProviderBase : Node{
    [Export]
    public bool UseObjectPool = true;
    [Export]
    public int PoolSize = 100;
    
}