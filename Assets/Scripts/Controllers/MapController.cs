using UnityEngine;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public MapModel mapModel;
    public MapView mapView;
    private List<PathNode> pathNodeList;
    private int currentPathIndex;

    public void Initialize(int width, int height, int cellSize)
    {
        mapView.Initialize(width, height, cellSize, new Vector2(-width * cellSize / 2, -height * cellSize / 2));
        mapModel = new MapModel(width, height);
    }

    public void HandleMouseClick(Vector3 mouseWorldPosition)
    {
        //mapModel.GetGridCoordinates(mouseWorldPosition, out int x, out int y);

        //Debug.Log($"X:{x} , Y:{y}");
        //List<PathNode> path = mapModel.FindPath(0, 0, x, y);

        //if (path != null)
        //{
        //    mapView.DrawPath(path);
        //    SetTargetPosition(mouseWorldPosition);
        //}
    }

    public void SetTargetPosition(int startX, int startY, int endX, int endY)
    {
        currentPathIndex = 0;
        pathNodeList = mapModel.GetPathfinding().FindPath(startX, startY, endX, endY);

        if (pathNodeList != null && pathNodeList.Count > 1)
        {
            pathNodeList.RemoveAt(0);
        }
    }
}
