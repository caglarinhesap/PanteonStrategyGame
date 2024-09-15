using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const int CELL_SIZE = 32;
    private const int MAP_WIDTH = 32;
    private const int MAP_HEIGHT = 32;
    public InfiniteScrollView infiniteScrollView;
    public MapController mapController;
    public InformationController informationController;
    public ProductionController productionController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mapController.Initialize(MAP_WIDTH, MAP_HEIGHT, CELL_SIZE);
    }

    private void Update()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

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

        if (Input.GetKey(KeyCode.Q))
        {
            mapController.SetTargetPosition(0, 0, 15, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            mapController.SetTargetPosition(15, 0, 0, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            mapController.mapModel.GetPathfinding().GetNode(15, 0).SetIsWalkable(false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            mapController.mapModel.GetPathfinding().GetNode(15, 0).SetIsWalkable(true);
        }
    }
}
