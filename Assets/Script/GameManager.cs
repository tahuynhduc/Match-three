using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : TemporaryMonoSingleton<GameManager>
{
    private BoardManager BoardManager => SingletonManager.BoardManager;
    private Matrix Matrix => BoardManager.boardMatrix;
    public int score;
    public GameObject prefab;
    [SerializeField] private Text scoreText;

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    private void Start()
    {
        InitBoard();
    }
    private void InitBoard()
    {
        Matrix.matrixItem.colItem = new List<Row<Item>>();
        for (var col = 0; col < Matrix.rowLength; col++)
        {
            var newColumn = new Row<Item>();
            var colPos = Matrix.GetSpaceCol(-col);
            for (var row = 0; row < Matrix.colLength; row++)
            {
                var rowPos = Matrix.GetSpaceRow(row);
                var position = new Vector2(rowPos, colPos);
                var cloneObj = Instantiate(prefab,position,Quaternion.identity);
                cloneObj.SetActive(true);
                var item = cloneObj.GetComponent<Item>();
                var color = this.RandomRange(new List<Type>() { Type.Red ,Type.Black,Type.Blue,Type.Cyan,Type.Green,Type.White,Type.Grey,Type.Yellow});
                item.UpdatePosition(row,col);
                item.UpdateColor(color);
                newColumn.rowItem.Add(item);
            }
            Matrix.matrixItem.colItem.Add(newColumn);
        }
    }

    public Item CloneItem(Vector2 position)
    {
        var cloneObj = Instantiate(prefab,position,Quaternion.identity);
        cloneObj.SetActive(true);
        return cloneObj.GetComponent<Item>();
    }
}
