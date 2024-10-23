using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : TemporaryMonoSingleton<BoardManager>
{
    public Matrix boardMatrix;
    public GameObject prefab;
    public bool isFalling;
    public Queue<Item> spareItem;

    
    private bool _isSelected;
    private Item _itemSelected;
    private bool _isSpawnItem;

    private void OnEnable()
    {
        EventManager.SelectedItem += OnSelectedItem;
    }
    private void Start()
    {
        InitBoard();
    }
    private void Update()
    {
        FallingItem();
        SpawnItem();
    }

    private void SpawnItem()
    {
        if(!_isSpawnItem) return;
        var colMatrixCount = boardMatrix.matrixItem.colItem;
        for (var row = 0; row <colMatrixCount[0].rowItem.Count; row++)
        {
            var rowPos = boardMatrix.GetSpaceRow(row);
            var topItem = boardMatrix.matrixItem.colItem[0].rowItem[row];
            if (topItem != null) continue;
            var position = new Vector2(rowPos, 0);
            var cloneObj = Instantiate(prefab,position,Quaternion.identity);
            cloneObj.SetActive(true);
            var item = cloneObj.GetComponent<Item>();
            var color = this.RandomRange(new List<Type>() { Type.Red ,Type.Black,Type.Blue,Type.Cyan,Type.Green,Type.White,Type.Grey,Type.Yellow});
            item.UpdatePosition(row,0);
            item.UpdateColor(color);
            colMatrixCount[0].rowItem[row] = item;
        }
        _isSpawnItem = false;
    }
    private void FallingItem()
    {
        StartCoroutine(BreakSecond());
    }

    private IEnumerator BreakSecond()
    {
        isFalling = false;
        var colMatrixCount = boardMatrix.matrixItem.colItem;
        // for (var col =0;col < colMatrixCount.Count;col++)
        for (var col = colMatrixCount.Count-1;col >=0 ;col--)
        {
            var colPos = boardMatrix.GetSpaceCol(-col);
            for (var row = 0; row <colMatrixCount[col].rowItem.Count; row++)
            {
                if (colMatrixCount[col].rowItem[row] == null)
                {
                    var rowPos = boardMatrix.GetSpaceRow(row);
                    var position = new Vector2(rowPos, colPos);
                    isFalling = true;
                    var topItem = boardMatrix.GetTop(row, col);
                    if (topItem == null) continue;
                    var xPos = (int)topItem.posMatrix.x;
                    var yPos = (int)topItem.posMatrix.y;
                    colMatrixCount[col].rowItem[row] = topItem;
                    var currentItem = colMatrixCount[col].rowItem[row];
                    boardMatrix.RemoveItem(xPos, yPos);
                    currentItem.UpdatePosition(row,col);
                    currentItem.MoveDown(position);
                    yield return null;
                }
            }
        }
        _isSpawnItem = true;
    }
    private void InitBoard()
    {
        boardMatrix.matrixItem.colItem = new List<Row<Item>>();
        for (var col = 0; col < boardMatrix.rowLength; col++)
        {
            var newColumn = new Row<Item>();
            var colPos = boardMatrix.GetSpaceCol(-col);
            for (var row = 0; row < boardMatrix.colLength; row++)
            {
                var rowPos = boardMatrix.GetSpaceRow(row);
                var position = new Vector2(rowPos, colPos);
                var cloneObj = Instantiate(prefab,position,Quaternion.identity);
                cloneObj.SetActive(true);
                var item = cloneObj.GetComponent<Item>();
                var color = this.RandomRange(new List<Type>() { Type.Red ,Type.Black,Type.Blue,Type.Cyan,Type.Green,Type.White,Type.Grey,Type.Yellow});
                item.UpdatePosition(row,col);
                item.UpdateColor(color);
                newColumn.rowItem.Add(item);
            }
            boardMatrix.matrixItem.colItem.Add(newColumn);
        }
    }
    private void OnSwapItem(Item swapItem)
    {
        var currentPos = new Vector2(_itemSelected.posMatrix.x,_itemSelected.posMatrix.y);
        var swapPos = new Vector2(swapItem.posMatrix.x, swapItem.posMatrix.y);
        boardMatrix.Swap(currentPos,swapPos);
    }
    private void OnSelectedItem(Item selectedItem)
    {
        _itemSelected = selectedItem;

    }
    public void GetNearItem(TouchDirection direction)
    {
        Item item;
        switch (direction)
        {
            case TouchDirection.Top:
                item = boardMatrix.GetTop(_itemSelected.posMatrix);
                break;
            case TouchDirection.Down:
                item = boardMatrix.GetDown(_itemSelected.posMatrix);
                break;
            case TouchDirection.Left:
                item = boardMatrix.GetLeft(_itemSelected.posMatrix);
                break;
            case TouchDirection.Right:
                item =  boardMatrix.GetRight(_itemSelected.posMatrix);
                break;
            default:
                OnSwapItem(_itemSelected);
                return;
        }
        OnSwapItem(item);
    }
}

public enum FallingType
{
    None =0,
    Vertical=1,
    Horizontal =2,
}
public class FallingEffect
{
    public static FallingEffect GetFallingType(FallingType type)
    {
        return type switch
        {
            FallingType.Horizontal => new HorizontalFalling(),
            FallingType.Vertical => new VerticalFalling(),
            _ => new FallingEffect()
        };
    } 
    public virtual void Falling()  { }
}

public class HorizontalFalling : FallingEffect
{
    public override void Falling()
    {
    }
}

public class VerticalFalling : FallingEffect
{
    public override void Falling()
    {
    }
}