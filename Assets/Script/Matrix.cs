using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Matrix
{
    //with xPos = col, yPos = row
    //get item by index need parameter col ---> row
    //get item by function need parameter row --->col
    public Col<Item> matrixItem;
    public int colLength;
    public int rowLength;
    public float spaceColumn;
    public float spaceRow;
    public void Swap(Vector2 currentPos,Vector2 swapPos)
    {
        var xPosCurrent = (int)currentPos.x;
        var yPosCurrent = (int)currentPos.y;
        var xPosSwap =  (int)swapPos.x;
        var yPosSwap =  (int)swapPos.y;
        (matrixItem.colItem[yPosCurrent].rowItem[xPosCurrent], matrixItem.colItem[yPosSwap].rowItem[xPosSwap]) = (matrixItem.colItem[yPosSwap].rowItem[xPosSwap], matrixItem.colItem[yPosCurrent].rowItem[xPosCurrent]);
        var currentItem = matrixItem.colItem[yPosCurrent].rowItem[xPosCurrent];
        var swapItem = matrixItem.colItem[yPosSwap].rowItem[xPosSwap];
        currentItem.UpdatePosition(xPosCurrent,yPosCurrent);
        swapItem.UpdatePosition(xPosSwap,yPosSwap);
        (currentItem.transform.position, swapItem.transform.position) = (swapItem.transform.position, currentItem.transform.position);
        EventManager.OnSwapItem(true);

    }
    public void GetNearItem(TouchDirection direction,Item itemSelected,Action<Item> callbackSwap = null)
    {
        Item item = null;
        switch (direction)
        {
            case TouchDirection.Top:
                item = GetTop(itemSelected.posMatrix);
                break;
            case TouchDirection.Down:
                item =GetDown(itemSelected.posMatrix);
                break;
            case TouchDirection.Left:
                item = GetLeft(itemSelected.posMatrix);
                break;
            case TouchDirection.Right:
                item = GetRight(itemSelected.posMatrix);
                break;
        }
        callbackSwap?.Invoke(item);
    }
    public Item GetLeft(Vector2 pos)
    {
        return GetLeft((int)pos.x, (int)pos.y);
    }
    public Item GetLeft(int x,int y)
    {
        var item =x > 0 ? matrixItem.colItem[y].rowItem[x - 1] :  GetCurrentItem(x,y);
        // Debug.LogError($"pos left {item.posMatrix}");
        return item;
    }
    public Item GetRight(Vector2 pos)
    {
        return GetRight((int)pos.x, (int)pos.y);
    }
    public Item GetRight(int x,int y)
    {
        var isOutOfRange = matrixItem.colItem[y].rowItem.Count - 1;
        var item = x < isOutOfRange ?  matrixItem.colItem[y].rowItem[x + 1] :  GetCurrentItem(x,y);
        // Debug.LogError($"pos Right {item.posMatrix}");
        return item;
    }
    public Item GetDown(Vector2 pos)
    {
        return GetDown((int)pos.x, (int)pos.y);
    }

    public Item GetDown(int x,int y)
    {
        var isOutOfRange =  matrixItem.colItem.Count -1 ;
        var item =  y < isOutOfRange? matrixItem.colItem[y + 1].rowItem[x] : GetCurrentItem(x,y);
        // Debug.LogError($"pos down {item.posMatrix}");
        return item;
    }
    public Item GetTop(Vector2 pos)
    {
        return GetTop((int)pos.x, (int)pos.y);
    }
    public Item GetTop(int x,int y)
    {
        var item = y > 0 ? matrixItem.colItem[y-1].rowItem[x] : GetCurrentItem(x,y);
        // Debug.LogError($"pos top {item.posMatrix}");
        return item;
    }
    public void RemoveItem(int x,int y)
    {
        matrixItem.colItem[y].rowItem[x] = null;
    }
    public Item GetCurrentItem(int x,int y)
    {
        return matrixItem.colItem[y].rowItem[x];
    }
    public float GetSpaceCol(int index)
    {
        return index * spaceColumn;
    }
    public float GetSpaceRow(int index)
    {
        return index * spaceRow;
    }
}