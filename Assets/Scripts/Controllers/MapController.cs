using UnityEngine;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public MapModel mapModel;
    public MapView mapView;
    private List<Vector3> pathVectorList;
    private int currentPathIndex;

    public void InitializeMap(int width, int height, int cellSize, Vector3 originPosition, GameObject squarePrefab, Transform mapSquaresParent)
    {
        mapModel = new MapModel(width, height, cellSize, originPosition);
        mapView.squarePrefab = squarePrefab;
        mapView.mapSquaresParent = mapSquaresParent;

        mapView.CreateGridVisuals(width, height, cellSize, new Vector2(-width * cellSize / 2, -height * cellSize / 2));
    }

    public void HandleMouseClick(Vector3 mouseWorldPosition)
    {
        mapModel.GetGridCoordinates(mouseWorldPosition, out int x, out int y);
        List<PathNode> path = mapModel.FindPath(0, 0, x, y);

        if (path != null)
        {
            mapView.DrawPath(path);
            SetTargetPosition(mouseWorldPosition);
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = mapModel.GetPathfinding().FindPath(Vector3.zero, targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}
