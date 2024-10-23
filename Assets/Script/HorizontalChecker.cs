using System;
using System.Collections.Generic;

[Serializable]
public class HorizontalChecker
{
    public List<Item> matchingList;
    protected Matrix BoardMatrix => SingletonManager.BoardManager.boardMatrix;

    public virtual void Check(Item firstItemInColumn)
    {
        var leftCheck = BoardMatrix.GetLeft(firstItemInColumn.posMatrix);
        var rightCheck = BoardMatrix.GetRight(firstItemInColumn.posMatrix);
        if (leftCheck !=null &&leftCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem && !leftCheck.isCheckedHorizontal )
        {
            if (leftCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedHorizontal = true;
                if(!matchingList.Contains(firstItemInColumn))
                    matchingList.Add(firstItemInColumn);
                if(!matchingList.Contains(leftCheck))
                    matchingList.Add(leftCheck);
                Check(leftCheck);
            }         
        } 
        if (  rightCheck !=null&&rightCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem && !rightCheck.isCheckedHorizontal)
        {
            if (rightCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedHorizontal = true;
                if(!matchingList.Contains(firstItemInColumn))
                    matchingList.Add(firstItemInColumn);
                if(!matchingList.Contains(rightCheck))
                    matchingList.Add(rightCheck);
                Check(rightCheck);
            }
        }
    }
}