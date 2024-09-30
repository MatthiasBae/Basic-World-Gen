using System;
using Godot;

public static class CoordinatesHelper {

    /// <summary>
    /// Calculates the sector of a position in the world. The position is in pixels.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector2I PositionToSector(Vector2 position) {
        // return new Vector2(position.X / chunkConfig.Fields.X), Mathf.FloorToInt(position.Y / chunkConfig.Fields.Y));
        // var fieldPosition = PositionToFieldPosition(position, fieldSize);
        // return FieldPositionToSector(fieldPosition, fieldCount);
        var sector = new Vector2I {
            X = Mathf.FloorToInt(position.X / ChunkController.FIELD_SIZE / ChunkController.CHUNK_SIZE),
            Y = Mathf.FloorToInt(position.Y / ChunkController.FIELD_SIZE / ChunkController.CHUNK_SIZE)
        };
        
        return sector;
    }

    public static Vector2 SectorToSectorCenterPosition(Vector2I sector) {
        return new Vector2 {
            X = sector.X * ChunkController.CHUNK_SIZE * ChunkController.FIELD_SIZE + ChunkController.CHUNK_SIZE * ChunkController.FIELD_SIZE / 2,
            Y = sector.Y * ChunkController.CHUNK_SIZE * ChunkController.FIELD_SIZE + ChunkController.CHUNK_SIZE * ChunkController.FIELD_SIZE / 2
        };
    }
    
    public static Vector2I FieldPositionToSector(Vector2I fieldPosition) {
        var sector = new Vector2I {
            X = Mathf.FloorToInt((float)fieldPosition.X / ChunkController.CHUNK_SIZE),
            Y = Mathf.FloorToInt((float)fieldPosition.Y / ChunkController.CHUNK_SIZE)
        };
        
        return sector;
    }
    
    public static Vector2I PositionToFieldPosition(Vector2 position) {
        return new Vector2I {
            X = Mathf.FloorToInt(position.X / ChunkController.FIELD_SIZE),
            Y = Mathf.FloorToInt(position.Y / ChunkController.FIELD_SIZE)
        };
    }
    
    /// <summary>
    /// Calculates the position of a sector in the world. The position is in pixels.
    /// </summary>
    /// <param name="sector"></param>
    /// <returns></returns>
    public static Vector2 SectorToFieldPosition(Vector2I sector) {
        return new Vector2 {
            X = sector.X * ChunkController.CHUNK_SIZE * ChunkController.FIELD_SIZE,
            Y = sector.Y * ChunkController.CHUNK_SIZE * ChunkController.FIELD_SIZE
        };
    }
    
    /// <summary>
    /// Calculates the local position of a FieldPosition in the world
    /// </summary>
    /// <param name="fieldPosition"></param>
    /// <param name="fieldCount"></param>
    /// <returns></returns>
    public static Vector2I FieldPositionToLocalPosition(Vector2I fieldPosition) {
        var localPosition = new Vector2I {
            X = fieldPosition.X % ChunkController.CHUNK_SIZE,
            Y = fieldPosition.Y % ChunkController.CHUNK_SIZE
        };
        
        if (localPosition.Y < 0) {
            localPosition.Y += ChunkController.CHUNK_SIZE;
        }
        
        if (localPosition.X < 0) {
            localPosition.X += ChunkController.CHUNK_SIZE;
        } 
        // var localPosition = new Vector2I {
        //     X = (fieldPosition.X + fieldCount.X) % fieldCount.X,
        //     Y = (fieldCount.Y - fieldPosition.Y - 1) % fieldCount.Y
        // };
        //
        // if (localPosition.Y < 0) {
        //     localPosition.Y += fieldCount.Y;
        // }
		      //
        // if (localPosition.X < 0) {
        //     localPosition.X += fieldCount.X;
        // }

        return localPosition;
    }
	
    public static Vector2I LocalPositionToFieldPosition(Vector2I chunkSector, Vector2I localPosition) {
        var fieldPosition = new Vector2I {
            X = chunkSector.X * ChunkController.CHUNK_SIZE + localPosition.X,
            Y = chunkSector.Y * ChunkController.CHUNK_SIZE + localPosition.Y
        };
        
        // if (fieldPosition.Y < 0) {
        //     fieldPosition.Y += fieldCount.Y;
        // }
        //
        // if (fieldPosition.X < 0) {
        //     fieldPosition.X += fieldCount.X;
        // }
        
        return fieldPosition;
    }
}