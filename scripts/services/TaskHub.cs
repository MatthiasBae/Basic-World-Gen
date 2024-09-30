using Godot;

public partial class TaskHub : Node {
    
    public static TaskHub Instance;
    
    public TaskGate ChunkGate = new();
    
    public override void _EnterTree() {
        Instance = this;
    }

    public override void _Process(double delta) {
        this.Update();
    }
    
    public void EnterChunkGate(ITask task) {
        this.ChunkGate.Add(task);
    }
    
    public void Update() {
        this.ChunkGate.Finish();
    }
}