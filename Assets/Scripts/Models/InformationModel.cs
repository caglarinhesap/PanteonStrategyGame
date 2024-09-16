using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class InformationModel
    {
        public SelectedUnit selectedUnit;

        public InformationModel()
        {
            selectedUnit = SelectedUnit.None;
        }
    }
}