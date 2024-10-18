using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MatchChecker : TemporaryMonoSingleton<MatchChecker>
{
    public Queue<Item> queueCheck;
    public List<Item> horizontalMatching;
    public List<Item> verticalMatching;
    private Matrix BoardMatrix => BoardManager.Instance.boardMatrix;
    public void CheckQueue()
    {
        queueCheck = new Queue<Item>();
        foreach (var colItem in BoardMatrix.matrixItem.colItem)
        {
            foreach (var item in colItem.rowItem)
            {
                item.isCheckedHorizontal = false;
                item.isCheckedVertical = false;
                queueCheck.Enqueue(item);

            }
        }
        while (queueCheck.Count >0)
        {
            var item = queueCheck.Dequeue();
            verticalMatching = new List<Item>();
            horizontalMatching = new List<Item>();
            CheckHorizontal(item);
            CheckVertical(item);
            if (verticalMatching.Count >= 3)
            {
                Debug.LogError($"matching verticalMatching:{verticalMatching.Count}");
                MatchingItems(verticalMatching);
            }
            
            if (horizontalMatching.Count >= 3)
            {
                Debug.LogError($"matching horizontalMatching :{horizontalMatching.Count}");
                MatchingItems(horizontalMatching);
            }
        }
    }
    private void MatchingItems(List<Item> matchingList)
    {
        foreach (var matchingItem in matchingList)
        {
            var topItem = BoardMatrix.GetTop(matchingItem.posMatrix);
            topItem.transform.position = matchingItem.transform.position;
            //BoardMatrix.Swap((int)topItem.posMatrix.x,(int)topItem.posMatrix.y,(int)matchingItem.posMatrix.x,(int)matchingItem.posMatrix.y);
            BoardMatrix.matrixItem.colItem[(int)matchingItem.posMatrix.y]
                .rowItem[(int)matchingItem.posMatrix.x] = topItem;
            matchingItem.DestroyObj();
        }
    }
    private void Update()
    {
        CheckQueue();
    }

    private void CheckHorizontal(Item firstItemInColumn)
    {
        var leftCheck = BoardMatrix.GetLeft(firstItemInColumn.posMatrix);
        var rightCheck = BoardMatrix.GetRight(firstItemInColumn.posMatrix);
        if (leftCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem && !leftCheck.isCheckedHorizontal)
        {
            if (leftCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedHorizontal = true;
                if(!horizontalMatching.Contains(firstItemInColumn))
                    horizontalMatching.Add(firstItemInColumn);
                if(!horizontalMatching.Contains(leftCheck))
                    horizontalMatching.Add(leftCheck);
                CheckHorizontal(leftCheck);
            }         
        } 
        else  if (rightCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem && !rightCheck.isCheckedHorizontal)
        {
            if (rightCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedHorizontal = true;
                if(!horizontalMatching.Contains(firstItemInColumn))
                    horizontalMatching.Add(firstItemInColumn);
                if(!horizontalMatching.Contains(rightCheck))
                    horizontalMatching.Add(rightCheck);
                CheckHorizontal(rightCheck);
            }
        }
    }

    private void CheckVertical(Item firstItemInColumn)
    {
        var topCheck = BoardMatrix.GetTop(firstItemInColumn.posMatrix);
        var downCheck = BoardMatrix.GetDown(firstItemInColumn.posMatrix);
        if (topCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem && !topCheck.isCheckedVertical)
        {
            if (topCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedVertical = true;
                if (!verticalMatching.Contains(firstItemInColumn))
                    verticalMatching.Add(firstItemInColumn);
                if (!verticalMatching.Contains(topCheck))
                 verticalMatching.Add(topCheck);
                CheckVertical(topCheck);
            }
        }
        else if (downCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem && !downCheck.isCheckedVertical)
        {
            if (downCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedVertical = true;
                if (!verticalMatching.Contains(firstItemInColumn))
                    verticalMatching.Add(firstItemInColumn);
                if (!verticalMatching.Contains(downCheck))
                    verticalMatching.Add(downCheck);
                CheckVertical(downCheck);
            }
        }
    }

    private IEnumerator CheckMatching()
    {
        yield return new WaitForSeconds(5f);
        CheckQueue();
    }
}