
using Godot;

public struct FieldTerrainRenderData {
    public Vector2I LocalPosition;
    public Vector2I FieldPosition;
    public int TileSetSourceId;
    public Vector2I AtlasCoordinate;
    public TilemapLayer.Layer Layer;
    
    public FieldTerrainRenderData(Vector2I fieldPosition, Vector2I localPosition, int tileSetSourceId, Vector2I atlasCoordinate, TilemapLayer.Layer layer) {
        this.FieldPosition = fieldPosition;
        this.TileSetSourceId = tileSetSourceId;
        this.AtlasCoordinate = atlasCoordinate;
        this.Layer = layer;
    }
}