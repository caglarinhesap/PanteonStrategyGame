using UnityEngine;
using System.Collections.Generic;
using Models;
using Views;

namespace Controllers
{
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

        public List<PathNode> SetTargetPosition(int startX, int startY, int endX, int endY)
        {
            pathNodeList = mapModel.GetPathfinding().FindPath(startX, startY, endX, endY);

            return pathNodeList;
        }
    }
}
