using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class ProductionModel
    {
        public SelectedBuilding selectedBuilding;

        public ProductionModel()
        {
            selectedBuilding = SelectedBuilding.None;
        }
    }
}