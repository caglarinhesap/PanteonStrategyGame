using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    [SerializeField] private GameObject dummyBuilding;
    public SelectedFrom selectedFrom;
    public UnitType unitType;
    public SelectedUnit selectedUnit;

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
            else if(selectedFrom == SelectedFrom.Map)
            {
                dummyBuilding.SetActive(false);
            }
        }
        else
        {
            dummyBuilding.SetActive(false);
        }
    }
}
