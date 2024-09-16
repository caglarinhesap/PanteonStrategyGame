using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class MapModel
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private Pathfinding pathFinding;

        public MapModel(int width, int height)
        {
            Width = width;
            Height = height;
            pathFinding = new Pathfinding(Width, Height);
        }

        public Pathfinding GetPathfinding()
        {
            return pathFinding;
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            return pathFinding.FindPath(startX, startY, endX, endY);
        }
    }
}