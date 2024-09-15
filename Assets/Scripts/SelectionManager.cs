using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    [SerializeField] private GameObject dummyBuilding;
    public SelectedFrom selectedFrom = SelectedFrom.Map;
    public SelectedUnit selectedUnit = SelectedUnit.None;
    public GameObject selectedMapObject = null;

    public List<GameObject> productionSquares = new List<GameObject>();

    private Color32 invalidColor = new Color32(154,0,0,255); //Inappropriate area color
    private Color32 validColor = new Color32(0,154,0,255); //Suitable area color
    private Color32 defaultColor = new Color32(84,84,84,255); //Default map color

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

    private void Update()
    {
        if (InputController.Instance.isMouseOnMap)
        {
            if (selectedFrom == SelectedFrom.ProductionPopup)
            {
                dummyBuilding.SetActive(true);
                dummyBuilding.transform.position =
                    GameManager.Instance.mapController.mapView.GetWorldPositionFromGrid(InputController.Instance.mouseGridPosition);

                if (selectedUnit == SelectedUnit.Barracks)
                {
                    dummyBuilding.transform.GetChild(0).gameObject.SetActive(true);
                    dummyBuilding.transform.GetChild(1).gameObject.SetActive(false);
                    SetDummyObjectBackground(BuildingType.Barracks, InputController.Instance.mouseGridPosition);
                }
                else if (selectedUnit == SelectedUnit.PowerPlant)
                {
                    dummyBuilding.transform.GetChild(0).gameObject.SetActive(false);
                    dummyBuilding.transform.GetChild(1).gameObject.SetActive(true);
                    SetDummyObjectBackground(BuildingType.PowerPlant, InputController.Instance.mouseGridPosition);
                }
            }
            else if (selectedFrom == SelectedFrom.Map)
            {
                dummyBuilding.SetActive(false);
                ResetOldSquares();
            }
        }
        else
        {
            dummyBuilding.SetActive(false); 
            ResetOldSquares();
        }
    }

    private void SetDummyObjectBackground(BuildingType building, Vector2 pivotGridPosition)
    {
        ResetOldSquares();
        bool isAreaSuitable = true;
        isAreaSuitable = CheckAreaSuitable(building, pivotGridPosition, isAreaSuitable);
        SetAreaColors(building, pivotGridPosition, isAreaSuitable);
    }

    private void SetAreaColors(BuildingType building, Vector2 pivotGridPosition, bool isAreaSuitable)
    {
        if (building == BuildingType.Barracks)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (isAreaSuitable)
                    {
                        if (GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGridPosition.x + i, pivotGridPosition.y + j)))
                        {
                            GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j].GetComponent<Image>().color = validColor;
                            productionSquares.Add(GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j]);
                        }
                    }
                    else
                    {
                        if (GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGridPosition.x + i, pivotGridPosition.y + j)))
                        {
                            GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j].GetComponent<Image>().color = invalidColor;
                            productionSquares.Add(GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j]);
                        }
                    }
                }
            }
        }
        else if (building == BuildingType.PowerPlant)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (isAreaSuitable)
                    {
                        if (GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGridPosition.x + i, pivotGridPosition.y + j)))
                        {
                            GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j].GetComponent<Image>().color = validColor;
                            productionSquares.Add(GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j]);
                        }
                    }
                    else
                    {
                        if (GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGridPosition.x + i, pivotGridPosition.y + j)))
                        {
                            GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j].GetComponent<Image>().color = invalidColor;
                            productionSquares.Add(GameManager.Instance.mapController.mapView.mapVisuals[(int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j]);
                        }
                    }
                }
            }
        }
    }

    private static bool CheckAreaSuitable(BuildingType building, Vector2 pivotGridPosition, bool isAreaSuitable)
    {
        if (building == BuildingType.Barracks)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGridPosition.x + i, pivotGridPosition.y + j)))
                    {
                        isAreaSuitable = false;
                        break;
                    }
                    else
                    {
                        if (!GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j).isWalkable)
                        {
                            isAreaSuitable = false;
                            break;
                        }
                    }
                }
            }
        }
        else if (building == BuildingType.PowerPlant)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGridPosition.x + i, pivotGridPosition.y + j)))
                    {
                        isAreaSuitable = false;
                        break;
                    }
                    else
                    {
                        if (!GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)pivotGridPosition.x + i, (int)pivotGridPosition.y + j).isWalkable)
                        {
                            isAreaSuitable = false;
                            break;
                        }
                    }
                }
            }
        }

        return isAreaSuitable;
    }

    private void ResetOldSquares()
    {
        foreach (GameObject square in productionSquares)
        {
            square.GetComponent<Image>().color = defaultColor;
        }
    }

    public void SelectMapObject(GameObject unit)
    {
        selectedMapObject = unit;
        FindUnitType(unit);
        SetVisuals();
    }

    public void Deselect()
    {
        selectedFrom = SelectedFrom.Map;
        selectedUnit = SelectedUnit.None;
        SetVisuals();
    }

    private void FindUnitType(GameObject unit)
    {
        if (unit.GetComponent<Barracks>() != null)
        {
            selectedUnit = SelectedUnit.Barracks;
        }
        else if (unit.GetComponent<PowerPlant>() != null)
        {
            selectedUnit = SelectedUnit.PowerPlant;
        }
        else if (unit.GetComponent<Soldier1>() != null)
        {
            selectedUnit = SelectedUnit.Soldier1;
        }
        else if (unit.GetComponent<Soldier2>() != null)
        {
            selectedUnit = SelectedUnit.Soldier2;
        }
        else if (unit.GetComponent<Soldier3>() != null)
        {
            selectedUnit = SelectedUnit.Soldier3;
        }
    }

    private void SetVisuals()
    {
        GameManager.Instance.informationController.SelectUnit(selectedUnit);

        switch (selectedUnit)
        {
            case SelectedUnit.None:
                GameManager.Instance.productionController.productionView.scrollView.SetBuildingScroll();
                break;
            case SelectedUnit.Barracks:
                GameManager.Instance.productionController.productionView.scrollView.SetProductionScroll();
                break;
            case SelectedUnit.PowerPlant:
                GameManager.Instance.productionController.productionView.scrollView.HideScroll();
                break;
            case SelectedUnit.Soldier1:
                GameManager.Instance.productionController.productionView.scrollView.SetBuildingScroll();
                break;
            case SelectedUnit.Soldier2:
                GameManager.Instance.productionController.productionView.scrollView.SetBuildingScroll();
                break;
            case SelectedUnit.Soldier3:
                GameManager.Instance.productionController.productionView.scrollView.SetBuildingScroll();
                break;
            default:
                GameManager.Instance.productionController.productionView.scrollView.SetBuildingScroll();
                break;
        }
    }
}
