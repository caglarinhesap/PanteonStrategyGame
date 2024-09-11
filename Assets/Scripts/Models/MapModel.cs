using System.Collections.Generic;
using UnityEngine;

public class MapModel
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int CellSize { get; private set; }
    public Vector3 OriginPosition { get; private set; }
    private Pathfinding pathFinding;

    public MapModel(int width, int height, int cellSize, Vector3 originPosition)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        OriginPosition = originPosition;
        pathFinding = new Pathfinding(Width, Height, CellSize, OriginPosition);
    }

    public Pathfinding GetPathfinding()
    {
        return pathFinding;
    }

    public void GetGridCoordinates(Vector3 worldPosition, out int x, out int y)
    {
        pathFinding.GetGrid().GetXY(worldPosition, out x, out y);
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        return pathFinding.FindPath(startX, startY, endX, endY);
    }
}
