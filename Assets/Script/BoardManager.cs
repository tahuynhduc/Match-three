using System.Collections.Generic;
using UnityEngine;

public class BoardManager : TemporaryMonoSingleton<BoardManager>
{
    public Matrix boardMatrix;
    public GameObject prefab;
    
    private const int IsMatchCount = 3;
    private bool _isSelected;
    private Item _itemSelected;
    private MatchChecker MatchChecker => MatchChecker.Instance;
    
    private void OnEnable()
    {
        EventManager.SelectedItem += OnSelectedItem;
    }
    private void Start()
    {
        InitBoard();
    }
    private void InitBoard()
    {
        boardMatrix.matrixItem.colItem = new List<Row<Item>>();
        for (var col = 0; col < boardMatrix.colLength; col++)
        {
            var newColumn = new Row<Item>();
            var colPos = boardMatrix.GetSpaceCol(-col);
            for (var row = 0; row < boardMatrix.rowLength; row++)
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
        boardMatrix.Swap((int)swapItem.posMatrix.x,(int)swapItem.posMatrix.y,(int)_itemSelected.posMatrix.x,(int)_itemSelected.posMatrix.y);
        (_itemSelected.transform.position, swapItem.transform.position) = (swapItem.transform.position, _itemSelected.transform.position);
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