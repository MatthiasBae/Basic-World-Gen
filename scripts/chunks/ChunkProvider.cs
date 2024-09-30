using Godot;
using System;

public partial class ChunkProvider : ObjectProviderBase {
    public static ChunkProvider Instance;
    
    public ObjectPool<Chunk> Pool;
    
    public override void _EnterTree() {
        Instance = this;
        this._InitializePool();
    }
    
    private void _InitializePool() {
        if (!this.UseObjectPool) {
            return;
        }

        this.Pool = new ObjectPool<Chunk>(this.PoolSize);
        for(var i = 0; i < this.PoolSize; i++) {
            var chunk = new Chunk();
            this.Pool.TryRecycle(chunk);
        }
    }

    public bool TryReleaseChunk(out Chunk chunk) {
        var released = false;
        chunk = null;
        if(this.UseObjectPool) {
            released = this.Pool.TryRelease(out chunk);
        }
        
        return released;
    }
    
    public bool TryRecycleChunk(Chunk chunk) {
        var recycled = false;
        if(!this.UseObjectPool) {
            recycled = this.Pool.TryRecycle(chunk);
        }
        
        return recycled;
    }
    
}
