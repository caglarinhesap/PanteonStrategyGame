using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public ObjectPool objectPool;
    public int visibleRowCount = 7;
    public int itemsPerRow = 2;
    public float itemHeight = 100f;
    public float itemWidth = 100f;
    private List<GameObject> activeItems = new List<GameObject>();
    private RectTransform contentRectTransform;
    private float itemSpacing = 10f;

    public List<ScrollItem> buildingList;
    public List<ScrollItem> productionList;

    private void Start()
    {
        contentRectTransform = scrollRect.content;
        PopulateItems();
    }

    private void Update()
    {
        CheckVisibility();
    }

    public void SetBuildingScroll()
    {
        gameObject.SetActive(true);
        UpdateScrollView(buildingList);
    }

    public void SetProductionScroll()
    {
        gameObject.SetActive(true);
        UpdateScrollView(productionList);
    }

    public void HideScroll()
    {
        gameObject.SetActive(false);
    }

    public void UpdateScrollView(List<ScrollItem> items)
    {
        List<GameObject> activeItems = new List<GameObject>();

        foreach (Transform child in scrollRect.content)
        {
            if (child.gameObject.activeInHierarchy)
            {
                objectPool.ReturnToPool(child.gameObject);
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = objectPool.GetPooledObject();
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            itemRectTransform.SetParent(scrollRect.content);
            itemRectTransform.localScale = Vector3.one;
            itemRectTransform.anchoredPosition = GetItemPosition(i);
            item.GetComponent<ScrollItem>().SetScrollItem(items[i]);
            activeItems.Add(item);
        }
    }

    private void PopulateItems()
    {
        for (int i = 0; i < visibleRowCount * itemsPerRow; i++)
        {
            GameObject item = objectPool.GetPooledObject();
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            itemRectTransform.SetParent(contentRectTransform);
            itemRectTransform.localScale = Vector3.one;

            //itemRectTransform.anchoredPosition = GetItemPosition(i);
            itemRectTransform.localPosition = GetItemPosition(i);
            activeItems.Add(item);
        }
    }

    private void CheckVisibility()
    {
        Vector3[] corners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(corners);
        float bottomEdge = corners[0].y;
        float topEdge = corners[2].y;

        List<GameObject> itemsToRemove = new List<GameObject>();
        List<GameObject> itemsToAdd = new List<GameObject>();

        foreach (GameObject item in activeItems)
        {
            RectTransform rectTransform = item.GetComponent<RectTransform>();
            if (rectTransform.anchoredPosition.y < bottomEdge || rectTransform.anchoredPosition.y > topEdge)
            {
                itemsToRemove.Add(item);
            }
        }

        foreach (GameObject item in itemsToRemove)
        {
            objectPool.ReturnToPool(item);
            activeItems.Remove(item);
            itemsToAdd.Add(objectPool.GetPooledObject());
        }

        foreach (GameObject newItem in itemsToAdd)
        {
            RectTransform newItemRectTransform = newItem.GetComponent<RectTransform>();
            newItemRectTransform.SetParent(contentRectTransform);
            newItemRectTransform.localScale = Vector3.one;
            newItemRectTransform.anchoredPosition = GetNewItemPosition();
            activeItems.Add(newItem);
        }
    }

    private Vector2 GetItemPosition(int index)
    {
        int row = index / itemsPerRow;
        int column = index % itemsPerRow;
        return new Vector2(column * (itemWidth + itemSpacing), -(row * (itemHeight + itemSpacing)));
    }

    Vector2 GetNewItemPosition()
    {
        RectTransform viewportRectTransform = scrollRect.viewport;

        Vector3[] viewportCorners = new Vector3[4];
        viewportRectTransform.GetWorldCorners(viewportCorners);

        float topEdge = viewportCorners[2].y;
        float bottomEdge = viewportCorners[0].y;

        float itemSpacing = 10;

        if (scrollRect.verticalNormalizedPosition > 0.5f) // Scroll up (Add element to the bottom)
        {
            return new Vector2(0, bottomEdge + itemHeight + itemSpacing);
        }
        else //Scroll down (Add element to the top)
        {
            return new Vector2(0, topEdge - itemHeight - itemSpacing);
        }
    }
}

