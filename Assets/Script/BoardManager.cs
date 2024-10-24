using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : TemporaryMonoSingleton<BoardManager>
{
    public Matrix boardMatrix;
    public bool isFalling;

    private bool _isSelected;
    private Item _itemSelected;
    private Item _itemSwapped;
    private bool _isSpawnItem;
    private GameManager GameManager => SingletonManager.GameManager;
    private PrefabsManager PrefabsManager => SingletonManager.PrefabsManager;

    private void OnEnable()
    {
        EventManager.SelectedItem += OnSelectedItem;
        EventManager.RevertItem += OnRevertItem;
    }
    private void Update()
    {
        FallingItem();
        SpawnItem();
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnItem()
    {
        if(!_isSpawnItem) return;
        var colMatrixCount = boardMatrix.matrixItem.colItem;
        for (var row = 0; row <colMatrixCount[0].rowItem.Count; row++)
        {
            var topItem = boardMatrix.matrixItem.colItem[0].rowItem[row];
            if (topItem != null) continue;
            var rowPos = boardMatrix.GetSpaceRow(row);
            var position = new Vector2(rowPos, 0);
            var item = GameManager.CloneItem(position);
            item.ActiveItem(true);
            item.UpdatePosition(row,0);
            item.MoveDown(position);
            item.UpdateColor(PrefabsManager.GetColor());
            colMatrixCount[0].rowItem[row] = item;
        }
        _isSpawnItem = false;
    }
    private void FallingItem()
    {
        isFalling = false;
        var colMatrixCount = boardMatrix.matrixItem.colItem;
        for (var col = colMatrixCount.Count-1;col >=0 ;col--)
        {
            var colPos = boardMatrix.GetSpaceCol(-col);
            for (var row = 0; row <colMatrixCount[col].rowItem.Count; row++)
            {
                if (colMatrixCount[col].rowItem[row] != null) continue;
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
            }  
        }
        _isSpawnItem = true;
    }

    public bool isSwap;
    private void OnSwapItem(Item swapItem)
    {
        isSwap = true;
        _itemSwapped = swapItem;
        boardMatrix.Swap(_itemSelected.posMatrix,_itemSwapped.posMatrix);
    }
    private void OnRevertItem()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        isSwap = false;
        EventManager.OnSwapItem(false);
        yield return new WaitForSeconds(0.2f);
        if(InputManager.direction == TouchDirection.Top || InputManager.direction == TouchDirection.Down )
            InputManager.direction = InputManager.direction == TouchDirection.Top ? TouchDirection.Down : TouchDirection.Top;
        else 
            InputManager.direction = InputManager.direction == TouchDirection.Right ? TouchDirection.Left : TouchDirection.Right;
        boardMatrix.Swap(_itemSelected.posMatrix,_itemSwapped.posMatrix);
    }
    private void OnSelectedItem(Item selectedItem)
    {
        _itemSelected = selectedItem;
    }
    public void GetNearItem(TouchDirection direction)
    {
        boardMatrix.GetNearItem(direction,_itemSelected,OnSwapItem);
    }
}