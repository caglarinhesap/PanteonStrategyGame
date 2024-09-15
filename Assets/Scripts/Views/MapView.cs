using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    public GameObject[,] mapVisuals;
    public GameObject squarePrefab;
    public Vector2 mapOriginPosition;
    public Vector2 mapEndPosition;
    public float cellDistance;

    public void Initialize(int width, int height, int cellSize, Vector2 startingPosition)
    {
        CreateGridVisuals(width, height, cellSize, startingPosition);
        SetPositions(width, height);
    }

    private void CreateGridVisuals(int width, int height, int cellSize, Vector2 startingPosition)
    {
        mapVisuals = new GameObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                mapVisuals[i, j] = Instantiate(squarePrefab, transform);

                RectTransform rectTransform = mapVisuals[i, j].GetComponent<RectTransform>();
                rectTransform.anchoredPosition = startingPosition + new Vector2(i * cellSize, j * cellSize);
            }
        }
    }

    private void SetPositions(float width, float height)
    {
        cellDistance = Mathf.Abs(Camera.main.ScreenToWorldPoint(mapVisuals[0, 0].transform.position).x - Camera.main.ScreenToWorldPoint(mapVisuals[1, 0].transform.position).x);
        Vector2 screenMiddlePoint = Camera.main.ScreenToWorldPoint(transform.position);
        mapOriginPosition = screenMiddlePoint - (new Vector2(width / 2f, height / 2f) * cellDistance);
        mapEndPosition = screenMiddlePoint + (new Vector2(width / 2f, height / 2f) * cellDistance);
    }

    public bool IsPositionInMap(Vector2 position) //Input should be screen to worldpoint
    {
        if (position.x >= mapOriginPosition.x && position.x <= mapEndPosition.x && position.y >= mapOriginPosition.y && position.y <= mapEndPosition.y)
        {
            return true;
        }
        return false;
    }

    public bool IsGridInMap(Vector2 grid)
    {
        if (grid.x >= 0 && grid.x < mapVisuals.GetLength(0) && grid.y >= 0 && grid.y < mapVisuals.GetLength(1))
        {
            return true;
        }
        return false;
    }

    public Vector2 GetGridFromPosition(Vector2 position) //Input should be screen to worldpoint
    {
        int gridX = Mathf.FloorToInt((position - mapOriginPosition).x / cellDistance);
        int gridY = Mathf.FloorToInt((position - mapOriginPosition).y / cellDistance);
        return new Vector2(gridX, gridY);
    }

    public Vector2 GetGridFromWorldPosition(Vector2 position) //Input should be world position
    {
        Vector2 screenPosition = Camera.main.ScreenToWorldPoint(position);
        int gridX = Mathf.RoundToInt((screenPosition - mapOriginPosition).x / cellDistance);
        int gridY = Mathf.RoundToInt((screenPosition - mapOriginPosition).y / cellDistance);
        return new Vector2(gridX, gridY);
    }

    public Vector2 GetWorldPositionFromGrid(Vector2 gridPosition)
    {
        Vector2 screenPosition = mapOriginPosition + (Vector2.one * cellDistance * gridPosition);
        return Camera.main.WorldToScreenPoint(screenPosition);
    }
}
