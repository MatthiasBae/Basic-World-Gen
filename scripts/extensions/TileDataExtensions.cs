using Godot;
using System;

public static class TileDataExtensions {
    public static PeeringBitMask GetTerrainPeeringBitMask(this TileData tileData) {
        var peeringBottomLeftCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.BottomLeftCorner);
        var peeringBottom = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.BottomSide);
        var peeringBottomRightCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.BottomRightCorner);
        var peeringTopLeftCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.TopLeftCorner);
        var peeringTop = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.TopSide);
        var peeringTopRightCorner = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.TopRightCorner);
        var peeringLeft = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.LeftSide);
        var peeringRight = tileData.GetTerrainPeeringBit(TileSet.CellNeighbor.RightSide);
        var peeringCenter = tileData.GetTerrain();
        
        return new PeeringBitMask {
            BottomLeft = peeringBottomLeftCorner,
            Bottom = peeringBottom,
            BottomRight = peeringBottomRightCorner,
            TopLeft = peeringTopLeftCorner,
            Top = peeringTop,
            TopRight = peeringTopRightCorner,
            Left = peeringLeft,
            Right = peeringRight,
            Center = peeringCenter
        };
    }
}
