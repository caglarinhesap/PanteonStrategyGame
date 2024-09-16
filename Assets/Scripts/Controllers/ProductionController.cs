using Enums;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Controllers
{
    public class ProductionController : MonoBehaviour
    {
        private ProductionModel productionModel = new ProductionModel();
        public ProductionView productionView;

        public void SetSelectedProductionBuilding(SelectedBuilding selectedBuilding)
        {
            productionModel.selectedBuilding = selectedBuilding;

            SelectionManager.Instance.selectedFrom = SelectedFrom.ProductionPopup;

            if (selectedBuilding == SelectedBuilding.Barracks)
            {
                SelectionManager.Instance.selectedUnit = SelectedUnit.Barracks;
            }
            else if (selectedBuilding == SelectedBuilding.PowerPlant)
            {
                SelectionManager.Instance.selectedUnit = SelectedUnit.PowerPlant;
            }
        }
    }
}