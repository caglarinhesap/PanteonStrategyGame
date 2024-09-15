using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    [SerializeField] private GameObject dummyBuilding;
    public SelectedFrom selectedFrom = SelectedFrom.Map;
    public SelectedUnit selectedUnit = SelectedUnit.None;
    public GameObject selectedMapObject = null;

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
                }
                else if (selectedUnit == SelectedUnit.PowerPlant)
                {
                    dummyBuilding.transform.GetChild(0).gameObject.SetActive(false);
                    dummyBuilding.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if (selectedFrom == SelectedFrom.Map)
            {
                dummyBuilding.SetActive(false);
            }
        }
        else
        {
            dummyBuilding.SetActive(false);
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
