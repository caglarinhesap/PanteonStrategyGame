using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class InfiniteScrollView : MonoBehaviour
    {
        public ScrollRect scrollRect;
        public ObjectPool objectPool;
        public int visibleRowCount = 3;
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
            SetBuildingScroll();
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
                itemRectTransform.anchoredPosition = GetItemPosition(i);
                activeItems.Add(item);
            }
        }

        private void CheckVisibility()
        {
            if (activeItems.Count <= visibleRowCount * itemsPerRow) // Not need to scroll.
            {
                return;
            }

            Vector3[] corners = new Vector3[4];
            scrollRect.viewport.GetWorldCorners(corners);
            float bottomEdge = corners[0].y;
            float topEdge = corners[2].y;

            List<GameObject> itemsToRemove = new List<GameObject>();
            List<GameObject> itemsToAdd = new List<GameObject>();

            bool isScrollingUp = scrollRect.velocity.y > 0;

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

                GameObject newItem = objectPool.GetPooledObject();
                RectTransform newItemRectTransform = newItem.GetComponent<RectTransform>();
                newItemRectTransform.SetParent(contentRectTransform);
                newItemRectTransform.localScale = Vector3.one;

                newItemRectTransform.anchoredPosition = GetNewItemPosition(isScrollingUp);

                activeItems.Add(newItem);
            }
        }


        private Vector2 GetItemPosition(int index)
        {
            int row = index / itemsPerRow;
            int column = index % itemsPerRow;
            return new Vector2(column * (itemWidth + itemSpacing), -(row * (itemHeight + itemSpacing)));
        }

        Vector2 GetNewItemPosition(bool isScrollingUp)
        {
            RectTransform contentRectTransform = scrollRect.content;

            float topMostItemY = float.MinValue;
            float bottomMostItemY = float.MaxValue;

            foreach (GameObject item in activeItems)
            {
                RectTransform rectTransform = item.GetComponent<RectTransform>();
                float itemY = rectTransform.anchoredPosition.y;

                if (itemY > topMostItemY)
                {
                    topMostItemY = itemY;
                }
                if (itemY < bottomMostItemY)
                {
                    bottomMostItemY = itemY;
                }
            }

            if (isScrollingUp)
            {
                return new Vector2(0, bottomMostItemY - (itemHeight + itemSpacing)); //Scroll up. (Add new item to the bottom)
            }
            else
            {
                return new Vector2(0, topMostItemY + (itemHeight + itemSpacing)); //Scroll down. (Add new item to the top)
            }
        }
    }
}