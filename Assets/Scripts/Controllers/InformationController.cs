using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationController : MonoBehaviour
{
    private InformationModel informationModel = new InformationModel();
    public InformationView informationView;

    private void Start()
    {
        SelectUnit(SelectedUnit.None);
    }

    public void SelectUnit(SelectedUnit unit)
    {
        informationModel.selectedUnit = unit;

        if (informationModel.selectedUnit == SelectedUnit.None)
        {
            informationView.HideSelectedUnitPanel();
            informationView.HideProductionPanel();
        }
        else
        {
            informationView.ShowSelectedUnitPanel(informationModel.selectedUnit);

            if (informationModel.selectedUnit == SelectedUnit.Barracks)
            {
                informationView.ShowProductionPanel();
            }
        }
    }
}
