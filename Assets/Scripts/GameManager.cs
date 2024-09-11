using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private const int CELL_SIZE = 32;
    private const int GRID_WIDTH = 34;
    private const int GRID_HEIGHT = 34;
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Transform mapSquaresParent;
    public InfiniteScrollView infiniteScrollView;
    public MapController mapController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mapController.InitializeMap(GRID_WIDTH, GRID_HEIGHT, CELL_SIZE, new Vector3(-GRID_WIDTH * CELL_SIZE / 2, -GRID_HEIGHT * CELL_SIZE / 2, 0), squarePrefab, mapSquaresParent);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();
            mapController.HandleMouseClick(mouseWorldPosition);
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
}
