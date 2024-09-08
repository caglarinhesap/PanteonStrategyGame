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

        }
        else if (unitType == UnitType.Production)
        {

        }
    }
}
