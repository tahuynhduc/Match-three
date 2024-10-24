using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : TemporaryMonoSingleton<GameManager>
{
    private BoardManager BoardManager => SingletonManager.BoardManager;
    private Matrix Matrix => BoardManager.boardMatrix;
    public int score;
    private GameObject Prefab => SingletonManager.PrefabsManager.prefab;
    private PrefabsManager PrefabsManager => SingletonManager.PrefabsManager;
    [SerializeField] private Text scoreText;
    
    private void Start()
    {
        InitBoard();
    }
    private void Update()
    {
        scoreText.text = score.ToString();
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
                var cloneObj = Instantiate(Prefab,position,Quaternion.identity);
                cloneObj.SetActive(true);
                var item = cloneObj.GetComponent<Item>();
                item.UpdatePosition(row,col);
                item.UpdateColor(PrefabsManager.GetColor());
                newColumn.rowItem.Add(item);
            }
            Matrix.matrixItem.colItem.Add(newColumn);
        }
    }
    public void UpdateScore(int count)
    {
        score += count;
    }

    public void StartGame()
    {
        score = 0;
    }
    public Item CloneItem(Vector2 position)
    {
        var cloneObj = Instantiate(Prefab,position,Quaternion.identity);
        cloneObj.SetActive(true);
        return cloneObj.GetComponent<Item>();
    }
}
