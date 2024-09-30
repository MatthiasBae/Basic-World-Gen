using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ChunkController : Node2D {
    
    public const int CHUNK_SIZE = 16;
    public const int FIELD_SIZE = 16;
    
    private Vector2I _AreaCenter;
    private Vector2I _AreaStart;
    private Vector2I _AreaEnd;

    private int _MaxAreaRadius;
    private Dictionary<ChunkProcessorBase.ChunkProcessState, int> Areas = new();
    private Dictionary<ChunkProcessorBase.ChunkProcessorStep, ChunkProcessorBase> Processors = new();
    
    private bool _ShallUpdateChunks;

    [Export]
    private Godot.Collections.Array<ChunkAreaConfig> _AreaConfigs;
    
    [ExportGroup("Child Nodes")]
    [Export]
    public ChunkStore ChunkStore;
    [Export]
    public Node ProcessorContainer;
    public override void _EnterTree() {
        this._Initialize();
    }

    public override void _Process(double delta) {
        if (!this._ShallUpdateChunks) {
            return;
        }
        
        this._ShallUpdateChunks = false;
        this._DistributeChunks();
    }

    private void _Initialize() {
        this._InitializeAreas();
        this._InitializeProcessors();
    }
    
    private void _InitializeAreas() {
        this._MaxAreaRadius = this._AreaConfigs.Max(area => area.Radius);

        foreach (var areaConfig in this._AreaConfigs) {
            this.Areas.TryAdd(
                areaConfig.ProcessState,
                areaConfig.Radius
            );
        }
    }
    
    private void _InitializeProcessors() {
        var children = this.ProcessorContainer.GetChildren();
        foreach (var child in children) {
            if (child is not ChunkProcessorBase processor) {
                continue;
            }
            
            this.Processors.TryAdd(processor.Step, processor);
        }
    }
    
    private void _UpdateArea(Vector2I enteredSector) {
        this._AreaCenter = enteredSector;
        this._AreaStart = enteredSector - new Vector2I(this._MaxAreaRadius, this._MaxAreaRadius);
        this._AreaEnd = enteredSector + new Vector2I(this._MaxAreaRadius, this._MaxAreaRadius);
    }
    
    private void _CreateChunks() {
        for(var x = this._AreaStart.X; x <= this._AreaEnd.X; x++) {
            for(var y = this._AreaStart.Y; y <= this._AreaEnd.Y; y++) {
                var sector = new Vector2I(x, y);
                if (this.ChunkStore.HasChunk(sector)) {
                    continue;
                }
                
                this._CreateChunk(sector);
            }
        }
    }
    
    private void _CreateChunk(Vector2I sector) {
        // var chunk = this.ChunkStore.CreateChunk(sector);
        var chunk = new Chunk() {
            Sector = sector
        };
        var added = this.ChunkStore.TryAddChunk(sector, chunk);
        if (added) {
            // GD.Print($"Created and added chunk: {sector}");
        }
    }
    
    //@TODO: Object pooling implementation
    private void _RemoveChunks() {
        GD.Print($"Before remove: Chunks in store: {this.ChunkStore.Chunks.Count()}");
        foreach(var chunk in this.ChunkStore.GetChunksEnumerable()) {
            if(this.IsChunkInArea(chunk.Sector)) {
                continue;
            }
            
            if (chunk.ProcessState > ChunkProcessorBase.ChunkProcessState.Prepared) {
                continue;
            }
            // GD.Print($"chunk {chunk.Sector} removed: {chunk.ProcessState}, {chunk.ProgressState}, ThreadLocked: {chunk.IsThreadLocked}");
            this.ChunkStore.RemoveChunk(chunk);
        }
    }

    private void _DistributeChunks() {
        var chunks = this.ChunkStore.GetChunksEnumerable();
        var processorsToStart = new List<ChunkProcessorBase.ChunkProcessorStep>();
        
        foreach (var chunk in chunks) {
            var skip = this.ShouldSkipDistribution(chunk);
            // Skip distribution if chunk is locked or already in progress
            // So "_ShallUpdateChunks" has to be true to check if chunk needs to be updated again
            if (skip) {
                //processor step needs to be added to the list because if the processor is using batch processing
                //chunks will always be InProgress and will never be updated again.
                //So we need to add the processor step to the list to make sure the processor is started again
                if (!processorsToStart.Contains(chunk.ProcessorStep)) {
                    processorsToStart.Add(chunk.ProcessorStep);
                }
                this._ShallUpdateChunks = true;
                continue;
            }
            
            var distributed = this._TryDistributeChunk(chunk, out var distributedToProcessorStep, out var needsUpdateAgain);
            if (!distributed) {
                continue;
            }
            
            if (needsUpdateAgain && this._ShallUpdateChunks == false) {
                this._ShallUpdateChunks = true;
            }
            
            if (!processorsToStart.Contains(distributedToProcessorStep)) {
                processorsToStart.Add(distributedToProcessorStep);
            }
        }
        
        this.StartProcessors(processorsToStart);
    }
    
    /// <summary>
    /// Distributes the chunk to the appropriate processor if necessary or possible
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="distributedToChunkProcessorStep"></param>
    /// <param name="needsUpdate"></param>
    /// <returns></returns>
    private bool _TryDistributeChunk(Chunk chunk, out ChunkProcessorBase.ChunkProcessorStep distributedToChunkProcessorStep, out bool needsUpdate) {
        distributedToChunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.None;
        needsUpdate = false;
        
        var distanceToCenter = new Vector2I {
            X = Math.Abs(chunk.Sector.X - this._AreaCenter.X),
            Y = Math.Abs(chunk.Sector.Y - this._AreaCenter.Y)
        };
        
        var maxDistanceToCenter = Math.Max(distanceToCenter.X, distanceToCenter.Y);
     
        var plannedProcessState = this.GetPlannedProcessState(maxDistanceToCenter);
        if(plannedProcessState == chunk.ProcessState) {
            return false;
        }
        
        var plannedProcessUpdateType = this.GetPlannedProcessUpdateType(chunk.ProcessState, plannedProcessState);
        var nextProcessState = this.GetNextProcessState(chunk.ProcessState, plannedProcessUpdateType);
        
        needsUpdate = nextProcessState != plannedProcessState;
        
        var found = this.TryGetProcessorStep(nextProcessState, plannedProcessUpdateType, out var processorStep);
        if (!found) {
            return false;
        }
        
        this.Processors[processorStep].QueueChunk(chunk);
        // GD.Print($"Distribution: {chunk.Sector} to {processorStep}");
        distributedToChunkProcessorStep = processorStep;
        return true;
    }
    
    private ChunkProcessorBase.ChunkProcessState GetPlannedProcessState(int distanceToCenter) {
        foreach (var area in this.Areas.OrderByDescending(a => a.Key)) {
            if (distanceToCenter <= area.Value) {
                return area.Key;
            }
        }
        return ChunkProcessorBase.ChunkProcessState.None;
    }

    private ChunkProcessorBase.ChunkProcessState GetNextProcessState(ChunkProcessorBase.ChunkProcessState chunkChunkProcessState, ChunkProcessorBase.ProcessorUpdateType plannedProcessUpdateType) {
        return chunkChunkProcessState + (int)plannedProcessUpdateType;
    }

    private ChunkProcessorBase.ProcessorUpdateType GetPlannedProcessUpdateType(ChunkProcessorBase.ChunkProcessState currentState, ChunkProcessorBase.ChunkProcessState plannedState) {
        var stateDiff = plannedState - currentState;
        switch (stateDiff) {
            case < 0:
                return ChunkProcessorBase.ProcessorUpdateType.Downgrade;
            case > 0:
                return ChunkProcessorBase.ProcessorUpdateType.Upgrade;
            default:
                return ChunkProcessorBase.ProcessorUpdateType.NoChange;
        }
    }
    private bool TryGetProcessorStep(ChunkProcessorBase.ChunkProcessState nextChunkProcessState, ChunkProcessorBase.ProcessorUpdateType plannedProcessUpdateType, out ChunkProcessorBase.ChunkProcessorStep chunkProcessorStep) {
        if(plannedProcessUpdateType == ChunkProcessorBase.ProcessorUpdateType.NoChange) {
            chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.None;
            return false;
        }
        else if (plannedProcessUpdateType == ChunkProcessorBase.ProcessorUpdateType.Upgrade) {
            switch (nextChunkProcessState) {
                case ChunkProcessorBase.ChunkProcessState.Prepared:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.Prepare;
                    return true;
                case ChunkProcessorBase.ChunkProcessState.Loaded:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.Load;
                    return true;
                case ChunkProcessorBase.ChunkProcessState.Rendered:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.Render;
                    return true;
                case ChunkProcessorBase.ChunkProcessState.SpawnedEntities:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.EntitySpawn;
                    return true;
                case ChunkProcessorBase.ChunkProcessState.SpawnedActors:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.ActorSpawn;
                    return true;
            }
        }
        else if (plannedProcessUpdateType == ChunkProcessorBase.ProcessorUpdateType.Downgrade) {
            switch (nextChunkProcessState) {
                case ChunkProcessorBase.ChunkProcessState.Prepared:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.Unload;
                    return true;
                case ChunkProcessorBase.ChunkProcessState.Loaded:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.Unrender;
                    return true;
                case ChunkProcessorBase.ChunkProcessState.Rendered:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.EntityDespawn;
                    return true;
                case ChunkProcessorBase.ChunkProcessState.SpawnedEntities:
                    chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.ActorDespawn;
                    return true;
            }
        }
        
        chunkProcessorStep = ChunkProcessorBase.ChunkProcessorStep.None;
        return false;
     }

    public bool ShouldSkipDistribution(Chunk chunk) {
        if(chunk.IsThreadLocked) {
            return true;
        }
        
        if (chunk.ProgressState == ChunkProcessorBase.ChunkProgressState.InProgress) {
            return true;
        }
        
        return false;
    }
    
    private void StartProcessors(List<ChunkProcessorBase.ChunkProcessorStep> processorSteps) {
        foreach(var processor in processorSteps) {
            this.Processors[processor].StartProcessor(out var chunksLeft);
            if (chunksLeft > 0) {
                this._ShallUpdateChunks = true;
                // GD.Print($"Started processor: {processor} with {chunksLeft} chunks left.");
            }
        }
    }
    
    public bool IsChunkInArea(Vector2I sector) {
        return sector.X >= this._AreaStart.X && sector.X <= this._AreaEnd.X && sector.Y >= this._AreaStart.Y && sector.Y <= this._AreaEnd.Y;
    }
    
    private void _OnPlayerTrackerSectorEntered(Vector2I enteredSector) {
        this._UpdateArea(enteredSector);
        this._CreateChunks();
        this._ShallUpdateChunks = true;
    }
    
    private void _OnPlayerTrackerSectorExited(Vector2I exitedSector) {
        this._RemoveChunks();
    }
}