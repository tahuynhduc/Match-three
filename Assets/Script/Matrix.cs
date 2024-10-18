using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Matrix
{
    public Col<Item> matrixItem;
    public int rowLength;
    public int colLength;
    public int isMatch;
    public float spaceColumn;
    public float spaceRow;
    public void Swap(int row1, int col1, int row2, int col2)
    {
        (matrixItem.colItem[col1].rowItem[row1], matrixItem.colItem[col2].rowItem[row2]) = (matrixItem.colItem[col2].rowItem[row2], matrixItem.colItem[col1].rowItem[row1]);
        matrixItem.colItem[col1].rowItem[row1].UpdatePosition(row1,col1); 
        matrixItem.colItem[col2].rowItem[row2].UpdatePosition(row2,col2); 
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

    private Item GetCurrentItem(int x,int y)
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