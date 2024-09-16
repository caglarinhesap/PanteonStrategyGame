using Controllers;
using UnityEngine;
using Views;

namespace Managers
{
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
    }
}