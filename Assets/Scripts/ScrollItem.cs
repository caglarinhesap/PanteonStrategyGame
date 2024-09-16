using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollItem : MonoBehaviour
{
    public Sprite itemSprite;
    public UnitType unitType;
    public ProductionType productionType;
    public BuildingType buildingType;
    [SerializeField] private Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(OnScrollItemClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnScrollItemClick);
    }

    public void SetScrollItem(ScrollItem item)
    {
        this.itemSprite = item.itemSprite;
        this.unitType = item.unitType;
        this.productionType = item.productionType;
        this.buildingType = item.buildingType;
        this.button.image.sprite = itemSprite;
    }

    private void OnScrollItemClick()
    {
        if (unitType == UnitType.Building)
        {
            if (buildingType == BuildingType.Barracks)
            {
                GameManager.Instance.productionController.SetSelectedProductionBuilding(SelectedBuilding.Barracks);
            }
            else if (buildingType == BuildingType.PowerPlant)
            {
                GameManager.Instance.productionController.SetSelectedProductionBuilding(SelectedBuilding.PowerPlant);
            }
        }
        else if (unitType == UnitType.Production)
        {
            Vector2 spawnPoint = SelectionManager.Instance.selectedMapObject.GetComponent<Barracks>().SpawnPoint;
            if (GameManager.Instance.mapController.mapView.IsGridInMap(spawnPoint))
            {
                if (GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)spawnPoint.x,(int)spawnPoint.y).isWalkable)
                {
                    GameObject createdSoldier;

                    switch (productionType)
                    {
                        case ProductionType.Soldier1:
                            createdSoldier = UnitFactory.Instance.CreateUnit(Units.Soldier1);
                            break;
                        case ProductionType.Soldier2:
                            createdSoldier = UnitFactory.Instance.CreateUnit(Units.Soldier2);
                            break;
                        case ProductionType.Soldier3:
                            createdSoldier = UnitFactory.Instance.CreateUnit(Units.Soldier3);
                            break;
                        default:
                            createdSoldier = UnitFactory.Instance.CreateUnit(Units.Soldier1);
                            break;
                    }

                    createdSoldier.GetComponent<IUnit>().OccupiedSquares.Add(new Vector2(spawnPoint.x, spawnPoint.y));

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (i == 1 && j == 1) //That point is inside of the barracks.
                            {
                                continue;
                            }

                            createdSoldier.GetComponent<IUnit>().SurroundingSquares.Add(new Vector2(spawnPoint.x + i - 1, spawnPoint.y + j - 1)); //Get surrounding squares.
                        }
                    }

                    createdSoldier.GetComponent<BaseSoldier>().CurrentPosition = new Vector2(spawnPoint.x, spawnPoint.y);

                    GameManager.Instance.mapController.mapModel.GetPathfinding().GetNode((int)spawnPoint.x, (int)spawnPoint.y).SetIsWalkable(false);
                    createdSoldier.transform.position = GameManager.Instance.mapController.mapView.GetWorldPositionFromGrid(spawnPoint);
                    UnitManager.Instance.Units.Add(createdSoldier);
                }
                else
                {
                    WarningController.Instance.ShowWarning("WARNING", "Spawn point is blocked.");
                }
            }
            else
            {
                WarningController.Instance.ShowWarning("WARNING", "Spawn point is outside the map.");
            }
        }
    }
}
