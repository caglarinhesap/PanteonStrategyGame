using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int CELL_SIZE = 32;
    private const int GRID_WIDTH = 34;
    private const int GRID_HEIGHT = 34;
    private GameObject[,] mapVisual = new GameObject[GRID_WIDTH, GRID_HEIGHT];
    private Pathfinding pathFinding;
    private Vector3 originPosition = new Vector3(-GRID_WIDTH * CELL_SIZE / 2, -GRID_HEIGHT * CELL_SIZE / 2, 0);
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Canvas canvas;
    private List<Vector3> pathVectorList;
    [SerializeField] private Transform mapSquaresParent;
    private int currentPathIndex;
    public InfiniteScrollView infiniteScrollView;


    private void Start()
    {
        pathFinding = new Pathfinding(GRID_WIDTH, GRID_HEIGHT, CELL_SIZE, originPosition);
        CreateGridVisuals();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();
            pathFinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
                }
            }

            SetTargetPosition(mouseWorldPosition);
        }

        if (Input.GetKey(KeyCode.C))
        {
            infiniteScrollView.SetBuildingScroll();
        }

        if (Input.GetKey(KeyCode.V))
        {
            infiniteScrollView.SetProductionScroll();
        }       
        
        if (Input.GetKey(KeyCode.B))
        {
            infiniteScrollView.HideScroll();
        }
    }

    private void CreateGridVisuals()
    {
        Vector2 startingPosition = new Vector2(-GRID_WIDTH * CELL_SIZE / 2, -GRID_HEIGHT * CELL_SIZE / 2);

        for (int i = 0; i < GRID_WIDTH; i++)
        {
            for (int j = 0; j < GRID_HEIGHT; j++)
            {
                mapVisual[i, j] = Instantiate(squarePrefab, mapSquaresParent);

                RectTransform rectTransform = mapVisual[i, j].GetComponent<RectTransform>();
                rectTransform.anchoredPosition = startingPosition + new Vector2(i * CELL_SIZE, j * CELL_SIZE);
            }
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}
