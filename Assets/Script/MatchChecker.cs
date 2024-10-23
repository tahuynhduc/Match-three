using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MatchChecker : TemporaryMonoSingleton<MatchChecker>
{
    public Queue<Item> queueCheck;
    public const int IsMatchLength = 3;
    private BoardManager BoardManager => SingletonManager.BoardManager;
    private GameManager GameManager => GameManager.Instance;
    private Matrix Matrix => BoardManager.boardMatrix;
    [SerializeField] private HorizontalChecker horizontalChecker;
    [SerializeField] private VerticalChecker verticalMatching;
    
    
    private void Update()
    {
        if(BoardManager.isFalling) return;
        CheckQueue();
    }

    private void CheckQueue()
    {
        queueCheck = new Queue<Item>();
        foreach (var colItem in Matrix.matrixItem.colItem)
        {
            foreach (var item in colItem.rowItem)
            {
                item.isCheckedHorizontal = false;
                item.isCheckedVertical = false;
                queueCheck.Enqueue(item);

            }
        }
        // Debug.LogError($"queueCheck.count{queueCheck.Count}");
        CheckMatchItem();
    }

    private void CheckMatchItem()
    {
        var isMatching = false;
        while (queueCheck.Count >0)
        {
            var item = queueCheck.Dequeue();
            horizontalChecker.matchingList = new List<Item>();
            verticalMatching.matchingList = new List<Item>();
            
            horizontalChecker.Check(item);
            verticalMatching.Check(item);
            
            if (verticalMatching.matchingList.Count >= IsMatchLength)
            {
                // Debug.LogError($"matching verticalMatching:{verticalMatching.Count}");
                MatchingItems(verticalMatching.matchingList);
                GameManager.score += verticalMatching.matchingList.Count;
                isMatching = true;
                BoardManager.isSwap = false;
            }
            if (horizontalChecker.matchingList.Count >= IsMatchLength)
            {
                // Debug.LogError($"matching horizontalMatching :{horizontalMatching.Count}");
                MatchingItems(horizontalChecker.matchingList);
                GameManager.score += horizontalChecker.matchingList.Count;
                isMatching = true;
                BoardManager.isSwap = false;
            }
        }
        if(isMatching) return;
        if(!BoardManager.isSwap) return;
        EventManager.OnRevertItem();
    }
    private void MatchingItems(List<Item> matchingList)
    {
        foreach (var matchingItem in matchingList)
        {
            var xPos = (int)matchingItem.posMatrix.x;
            var yPos = (int)matchingItem.posMatrix.y;
            Matrix.RemoveItem(xPos,yPos);
            matchingItem.ActiveItem();
        }
    }
    
}