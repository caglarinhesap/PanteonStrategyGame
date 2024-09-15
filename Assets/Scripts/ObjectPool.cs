using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject itemPrefab;
    public int poolSize = 3;
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(itemPrefab,transform);
            item.SetActive(false);
            pool.Enqueue(item);
        }
    }

    public GameObject GetPooledObject()
    {
        if (pool.Count > 0)
        {
            GameObject item = pool.Dequeue();
            item.SetActive(true);
            return item;
        }
        else
        {
            GameObject item = Instantiate(itemPrefab,transform);
            return item;
        }
    }

    public void ReturnToPool(GameObject item)
    {
        item.SetActive(false);
        pool.Enqueue(item);
    }
}

