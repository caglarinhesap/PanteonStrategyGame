using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    public List<GameObject> Units;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject FindUnit(Vector2 clickedGrid)
    {
        foreach (GameObject unit in Units)
        {
            foreach (Vector2 square in unit.GetComponent<IUnit>().OccupiedSquares)
            {
                if (clickedGrid == square)
                {
                    return unit;
                }
            }
        }

        return null;
    }
}
