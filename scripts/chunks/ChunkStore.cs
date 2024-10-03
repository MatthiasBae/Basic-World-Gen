using Godot;
using System;
using System.Collections.Generic;

public partial class ChunkStore : Node {
    public static ChunkStore Instance;
    
    [Export]
    public ChunkProvider ChunkProvider;
    
    public Dictionary<Vector2I, Chunk> Chunks = new();
    
    public override void _EnterTree() {
        Instance = this;
    }
    
    public Chunk CreateChunk(Vector2I sector) {
        var released = this.ChunkProvider.TryReleaseChunk(out var chunk);
        if (!released) {
            return new Chunk() {
                Sector = sector
            };
        }

        chunk.Sector = sector;
        return chunk;
    }
    
    public bool TryAddChunk(Vector2I sector, Chunk chunk) {
        return this.Chunks.TryAdd(sector, chunk);
    }
    
    public bool TryGetChunk(Vector2I sector, out Chunk chunk) {
        return this.Chunks.TryGetValue(sector, out chunk);
    }
    
    public IEnumerable<Chunk> GetChunksEnumerable() {
        foreach(var chunk in this.Chunks.Values) {
            yield return chunk;
        }
    }

    public void RemoveChunk(Chunk chunk) {
        var recycled = this.ChunkProvider.TryRecycleChunk(chunk);
        this.Chunks.Remove(chunk.Sector);
    }
    
    public bool HasChunk(Vector2I sector) {
        return this.Chunks.ContainsKey(sector);
    }
    
    public static bool TryGetField(Vector2I fieldPosition, out Field field) {
        var sector = CoordinatesHelper.FieldPositionToSector(fieldPosition);
        var localPosition = CoordinatesHelper.FieldPositionToLocalPosition(fieldPosition);
        
        var chunkExists = Instance.TryGetChunk(sector, out var chunk);
        if(chunkExists) {
            field = chunk.Fields[localPosition.X, localPosition.Y];
            return true;
        }
        
        field = null;
        return false;
    }
    
    public static Field GetField(Vector2I fieldPosition) {
        var sector = CoordinatesHelper.FieldPositionToSector(fieldPosition);
        var localPosition = CoordinatesHelper.FieldPositionToLocalPosition(fieldPosition);
        
        var chunkExists = Instance.TryGetChunk(sector, out var chunk);
        if(chunkExists) {
            var field = chunk.Fields[localPosition.X, localPosition.Y];
            return field;
        }
        
        return null;
    }
    
    public static Field[,] GetSurroundingFields(Vector2I fieldPosition) {
        var surroundingFields = new Field[,] {
            {
                GetField(fieldPosition + NeighborCoordinate.TOPLEFT_OFFSET),
                GetField(fieldPosition + NeighborCoordinate.LEFT_OFFSET),
                GetField(fieldPosition + NeighborCoordinate.BOTTOMLEFT_OFFSET)
            }, {
                GetField(fieldPosition + NeighborCoordinate.TOP_OFFSET),
                GetField(fieldPosition),
                GetField(fieldPosition + NeighborCoordinate.BOTTOM_OFFSET)
            }, {
                GetField(fieldPosition + NeighborCoordinate.TOPRIGHT_OFFSET),
                GetField(fieldPosition + NeighborCoordinate.RIGHT_OFFSET),
                GetField(fieldPosition + NeighborCoordinate.BOTTOMRIGHT_OFFSET)
            }
        };
        return surroundingFields;
        
        // var surroundingFields = new Field[3, 3];
        // for(var x = 0; x < 3; x++) {
        //     for(var y = 0; y < 3; y++) {
        //         var neighborPosition = fieldPosition + new Vector2I(x - 1, y - 1);
        //         var field = GetField(neighborPosition);
        //         surroundingFields[x, y] = field;
        //     }
        // }
        // return surroundingFields;
    }
}
