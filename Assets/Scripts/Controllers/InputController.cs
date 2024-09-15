using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    public Vector2 mouseGridPosition;
    public bool isMouseOnMap = false;

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
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (GameManager.Instance.mapController.mapView.IsPositionInMap(mousePosition))
        {
            isMouseOnMap = true;
            mouseGridPosition = GameManager.Instance.mapController.mapView.GetGridFromPosition(mousePosition);

            if (Input.GetMouseButtonDown(0)) //Mouse left click
            {
                if (SelectionManager.Instance.selectedFrom == SelectedFrom.ProductionPopup)
                {
                    if (SelectionManager.Instance.selectedUnit == SelectedUnit.Barracks)
                    {
                        if (CanBuild(BuildingType.Barracks, mouseGridPosition))
                        {
                            Build(BuildingType.Barracks, mouseGridPosition);
                        }
                    }
                    else if (SelectionManager.Instance.selectedUnit == SelectedUnit.PowerPlant)
                    {
                        if (CanBuild(BuildingType.PowerPlant, mouseGridPosition))
                        {
                            Build(BuildingType.PowerPlant, mouseGridPosition);
                        }
                    }
                }
                else if (SelectionManager.Instance.selectedFrom == SelectedFrom.Map)
                {
                    GameObject unit = UnitManager.Instance.FindUnit(mouseGridPosition);
                    if (unit != null)
                    {
                        SelectionManager.Instance.SelectMapObject(unit);
                    }
                    else
                    {
                        SelectionManager.Instance.Deselect();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1)) //Mouse right click
            {
                if (SelectionManager.Instance.selectedMapObject.GetComponent<BaseSoldier>() != null)
                {
                    if (!UnitManager.Instance.FindUnit(mouseGridPosition)) //Target grid is empty. Move
                    {
                        Vector2 currentLocation = GameManager.Instance.mapController.mapView.GetGridFromWorldPosition(SelectionManager.Instance.selectedMapObject.transform.position);
                        SelectionManager.Instance.selectedMapObject.GetComponent<BaseSoldier>().SetPath(currentLocation, mouseGridPosition);
                    }
                    else //Target grid is occupied. Attack!
                    {

                    }
                }
            }
        }
        else
        {
            isMouseOnMap = false;
        }
    }

    public bool CanBuild(BuildingType building, Vector2 pivotGrid)
    {
        bool result = true;

        switch (building)
        {
            case BuildingType.Barracks:
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGrid.x + i, pivotGrid.y + j)))
                        {
                            if (GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)pivotGrid.x + i, (int)pivotGrid.y + j).isWalkable)
                            {
                                //result = true;
                            }
                            else
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }
                }
                break;
            case BuildingType.PowerPlant:
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (GameManager.Instance.mapController.mapView.IsGridInMap(new Vector2(pivotGrid.x + i, pivotGrid.y + j)))
                        {
                            if (GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)pivotGrid.x + i, (int)pivotGrid.y + j).isWalkable)
                            {
                                //result = true;
                            }
                            else
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }
                }
                break;
        }

        return result;
    }

    private void Build(BuildingType building, Vector2 pivotGrid)
    {
        GameObject createdBuilding;

        if (building == BuildingType.Barracks)
        {
            createdBuilding = UnitFactory.Instance.CreateUnit(Units.Barracks);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    createdBuilding.GetComponent<Barracks>().OccupiedSquares.Add(new Vector2(pivotGrid.x + i, pivotGrid.y + j));
                    GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)pivotGrid.x + i, (int)pivotGrid.y + j).SetIsWalkable(false);
                }
            }

            // Set the spawn point to the bottom square. Ensure the spawn point is within the map and empty while spawning.
            createdBuilding.GetComponent<Barracks>().SpawnPoint = new Vector2(pivotGrid.x, pivotGrid.y - 1);
        }
        else
        {
            createdBuilding = UnitFactory.Instance.CreateUnit(Units.PowerPlant);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    createdBuilding.GetComponent<PowerPlant>().OccupiedSquares.Add(new Vector2(pivotGrid.x + i, pivotGrid.y + j));
                    GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)pivotGrid.x + i, (int)pivotGrid.y + j).SetIsWalkable(false);
                }
            }
        }

        createdBuilding.transform.position = GameManager.Instance.mapController.mapView.GetWorldPositionFromGrid(pivotGrid);
        UnitManager.Instance.Units.Add(createdBuilding);
        SelectionManager.Instance.Deselect();
    }
}
