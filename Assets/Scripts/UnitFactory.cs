using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFactory : MonoBehaviour
{
    public static UnitFactory Instance { get; private set; }
    public ObjectPool unitPool;
    [SerializeField] private Transform UnitsParent;
    [SerializeField] private Sprite barrackSprite;
    [SerializeField] private Sprite powerPlantSprite;
    [SerializeField] private Sprite soldier1Sprite;
    [SerializeField] private Sprite soldier2Sprite;
    [SerializeField] private Sprite soldier3Sprite;

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

    public GameObject CreateUnit(Units unit)
    {
        GameObject createdUnit = unitPool.GetPooledObject();
        switch (unit)
        {
            case Units.Barracks:
                createdUnit.GetComponent<RectTransform>().sizeDelta = new Vector2(128, 128);
                createdUnit.GetComponent<Image>().sprite = barrackSprite;
                createdUnit.AddComponent<Barracks>();
                break;
            case Units.PowerPlant:
                createdUnit.GetComponent<RectTransform>().sizeDelta = new Vector2(64, 96);
                createdUnit.GetComponent<Image>().sprite = powerPlantSprite;
                createdUnit.AddComponent<PowerPlant>();
                break;
            case Units.Soldier1:
                createdUnit.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);
                createdUnit.GetComponent<Image>().sprite = soldier1Sprite;
                createdUnit.AddComponent<Soldier1>();
                break;
            case Units.Soldier2:
                createdUnit.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);
                createdUnit.GetComponent<Image>().sprite = soldier2Sprite;
                createdUnit.AddComponent<Soldier2>();
                break;
            case Units.Soldier3:
                createdUnit.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);
                createdUnit.GetComponent<Image>().sprite = soldier3Sprite;
                createdUnit.AddComponent<Soldier3>();
                break;
            default:
                break;
        }

        createdUnit.transform.SetParent(UnitsParent);
        createdUnit.GetComponent<RectTransform>().localScale = Vector3.one;
        return createdUnit;
    }

    public void DestroyUnit(GameObject unit)
    {
        RemoveComponentsFromUnit(unit);
        unit.transform.SetParent(unitPool.transform);
        UnitManager.Instance.Units.Remove(unit);
        unitPool.ReturnToPool(unit);
    }

    private void RemoveComponentsFromUnit(GameObject unit)
    {
        Soldier1 soldier1 = unit.GetComponent<Soldier1>();
        Soldier2 soldier2 = unit.GetComponent<Soldier2>();
        Soldier3 soldier3 = unit.GetComponent<Soldier3>();
        Barracks barracks = unit.GetComponent<Barracks>();
        PowerPlant powerplant = unit.GetComponent<PowerPlant>();

        if (soldier1 != null)
            Destroy(soldier1);       
        
        if (soldier2 != null)
            Destroy(soldier2);     
        
        if (soldier3 != null)
            Destroy(soldier3);      
        
        if (barracks != null)
            Destroy(barracks);        

        if (powerplant != null)
            Destroy(powerplant);
    }

}