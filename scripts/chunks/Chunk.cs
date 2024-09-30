using Godot;
using System;
using Godot.Collections;

public class Chunk : IPoolableObject {
    public Vector2I Sector;
    public bool IsThreadLocked;
    public Field[,] Fields;
    
    public ChunkProcessorBase.ChunkProcessState ProcessState = ChunkProcessorBase.ChunkProcessState.None;
    public ChunkProcessorBase.ChunkProgressState ProgressState = ChunkProcessorBase.ChunkProgressState.NotStarted;
    public ChunkProcessorBase.ChunkProcessorStep ProcessorStep;

    public void Reset() {
        this.Sector = Vector2I.Zero;
        this.IsThreadLocked = false;
        this.Fields = null;
        
        this.ProcessState = ChunkProcessorBase.ChunkProcessState.None;
        this.ProgressState = ChunkProcessorBase.ChunkProgressState.NotStarted;
        this.ProcessorStep = ChunkProcessorBase.ChunkProcessorStep.None;
    }

    public void Initialize() {
        
    }
}

