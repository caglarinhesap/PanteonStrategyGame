using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    private GameObject[,] mapVisuals;
    public GameObject squarePrefab;
    public Transform mapSquaresParent;

    public void CreateGridVisuals(int width, int height, int cellSize, Vector2 startingPosition)
    {
        mapVisuals = new GameObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                mapVisuals[i, j] = Instantiate(squarePrefab, mapSquaresParent);

                RectTransform rectTransform = mapVisuals[i, j].GetComponent<RectTransform>();
                rectTransform.anchoredPosition = startingPosition + new Vector2(i * cellSize, j * cellSize);
            }
        }
    }

    public void DrawPath(List<PathNode> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
        }
    }
}
