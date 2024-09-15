using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationView : MonoBehaviour
{
    [SerializeField] private GameObject selectedUnitPanel;
    [SerializeField] private TextMeshProUGUI selectedUnitName;
    [SerializeField] private Image selectedUnitImage;
    [SerializeField] private GameObject productionPanel;

    [SerializeField] private Sprite barracksSprite;
    [SerializeField] private Sprite powerPlantSprite;
    [SerializeField] private Sprite soldier1Sprite;
    [SerializeField] private Sprite soldier2Sprite;
    [SerializeField] private Sprite soldier3Sprite;


    public void ShowSelectedUnitPanel(SelectedUnit selectedUnit)
    {
        selectedUnitName.text = selectedUnit.ToString().ToUpper().Replace("Ý","I");

        switch (selectedUnit)
        {
            case SelectedUnit.None:
                break;
            case SelectedUnit.Barracks:
                selectedUnitImage.sprite = barracksSprite;
                break;
            case SelectedUnit.PowerPlant:
                selectedUnitImage.sprite = powerPlantSprite;
                break;
            case SelectedUnit.Soldier1:
                selectedUnitImage.sprite = soldier1Sprite;
                break;
            case SelectedUnit.Soldier2:
                selectedUnitImage.sprite = soldier2Sprite;
                break;
            case SelectedUnit.Soldier3:
                selectedUnitImage.sprite = soldier3Sprite;
                break;
            default:
                break;
        }

        selectedUnitPanel.SetActive(true);
    }
    public void HideSelectedUnitPanel()
    {
        selectedUnitPanel.SetActive(false);
    }
    public void ShowProductionPanel()
    {
        productionPanel.SetActive(true);
    }

    public void HideProductionPanel()
    {
        productionPanel.SetActive(false);
    }
}
