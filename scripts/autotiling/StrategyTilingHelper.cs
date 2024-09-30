
using System;
using System.Linq;
using Godot;

public class StrategyTilingHelper {
    public static bool IsEdgeTile(TileData tileData) {
        var peeringBottomLeftCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.BottomLeftCorner);
        var peeringBottom = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.BottomSide);
        var peeringBottomRightCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.BottomRightCorner);
        var peeringTopLeftCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.TopLeftCorner);
        var peeringTop = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.TopSide);
        var peeringTopRightCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.TopRightCorner);
        var peeringLeft = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.LeftSide);
        var peeringRight = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.RightSide);
        var peeringCenter = tileData.GetTerrain();
        
        return peeringBottomLeftCorner < 0 || peeringBottom < 0 || peeringBottomRightCorner < 0
               || peeringTopLeftCorner < 0 || peeringTop < 0 || peeringTopRightCorner < 0
               || peeringLeft < 0 || peeringCenter < 0 || peeringRight < 0;
        
    }
    
    public static int GetBinaryValue(BitMask bitMask, PresenceMask presenceMask) {
        var topLeftReset = presenceMask.Left <= 0 || presenceMask.Top <= 0;
        var topRightReset = presenceMask.Right <= 0 || presenceMask.Top <= 0;
        var bottomLeftReset = presenceMask.Left <= 0 || presenceMask.Bottom <= 0;
        var bottomRightReset = presenceMask.Right <= 0 || presenceMask.Bottom <= 0;
        
        var bitMaskTopLeft = topLeftReset ? 0 : bitMask.TopLeft;
        var bitMaskTopRight = topRightReset ? 0 : bitMask.TopRight;
        var bitMaskBottomLeft = bottomLeftReset ? 0 : bitMask.BottomLeft;
        var bitMaskBottomRight = bottomRightReset ? 0 : bitMask.BottomRight;

        return bitMaskTopLeft * presenceMask.TopLeft +
               bitMaskTopRight * presenceMask.TopRight +
               bitMaskBottomLeft * presenceMask.BottomLeft +
               bitMaskBottomRight * presenceMask.BottomRight +
               bitMask.Left * presenceMask.Left +
               bitMask.Top * presenceMask.Top +
               bitMask.Right * presenceMask.Right +
               bitMask.Bottom * presenceMask.Bottom +
               bitMask.Center * presenceMask.Center;

    }
    
    public static TerrainMask GetTerrainMask(FieldTerrainLayer[,] neighborsFieldTerrainLayer) {
        var topLeftTerrain = neighborsFieldTerrainLayer[0, 0] != null && neighborsFieldTerrainLayer[0, 0].HasTerrain;
        var topTerrain = neighborsFieldTerrainLayer[1, 0] != null && neighborsFieldTerrainLayer[1, 0].HasTerrain;
        var topRightTerrain = neighborsFieldTerrainLayer[2, 0] != null && neighborsFieldTerrainLayer[2, 0].HasTerrain;
        var leftTerrain = neighborsFieldTerrainLayer[0, 1] != null && neighborsFieldTerrainLayer[0, 1].HasTerrain;
        var centerTerrain = neighborsFieldTerrainLayer[1, 1] != null && neighborsFieldTerrainLayer[1, 1].HasTerrain;
        var rightTerrain = neighborsFieldTerrainLayer[2, 1] != null && neighborsFieldTerrainLayer[2, 1].HasTerrain;
        var bottomLeftTerrain = neighborsFieldTerrainLayer[0, 2] != null && neighborsFieldTerrainLayer[0, 2].HasTerrain;
        var bottomTerrain = neighborsFieldTerrainLayer[1, 2] != null && neighborsFieldTerrainLayer[1, 2].HasTerrain;
        var bottomRightTerrain = neighborsFieldTerrainLayer[2, 2] != null && neighborsFieldTerrainLayer[2, 2].HasTerrain;
        
        
        return new TerrainMask {
            TopLeft = topLeftTerrain ? neighborsFieldTerrainLayer[0, 0].Config.AtlasSourceId : -1,
            Top = topTerrain ? neighborsFieldTerrainLayer[1, 0].Config.AtlasSourceId : -1,
            TopRight = topRightTerrain ? neighborsFieldTerrainLayer[2, 0].Config.AtlasSourceId : -1,
            Left = leftTerrain ? neighborsFieldTerrainLayer[0, 1].Config.AtlasSourceId : -1,
            Center = centerTerrain ? neighborsFieldTerrainLayer[1, 1].Config.AtlasSourceId : -1,
            Right = rightTerrain ? neighborsFieldTerrainLayer[2, 1].Config.AtlasSourceId : -1,
            BottomLeft = bottomLeftTerrain ? neighborsFieldTerrainLayer[0, 2].Config.AtlasSourceId : -1,
            Bottom = bottomTerrain ? neighborsFieldTerrainLayer[1, 2].Config.AtlasSourceId : -1,
            BottomRight = bottomRightTerrain ? neighborsFieldTerrainLayer[2, 2].Config.AtlasSourceId : -1
        };
    }
}