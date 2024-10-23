using System;

[Serializable]
public class VerticalChecker : HorizontalChecker
{
    public override void Check(Item firstItemInColumn)
    {
        var topCheck = BoardMatrix.GetTop(firstItemInColumn.posMatrix);
        var downCheck = BoardMatrix.GetDown(firstItemInColumn.posMatrix);
        if (  topCheck !=null&&topCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem && !topCheck.isCheckedVertical)
        {
            if (topCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedVertical = true;
                if (!matchingList.Contains(firstItemInColumn))
                    matchingList.Add(firstItemInColumn);
                if (!matchingList.Contains(topCheck))
                    matchingList.Add(topCheck);
                Check(topCheck);
            }
        }
        if (  downCheck !=null&&downCheck.currentColor.typeItem == firstItemInColumn.currentColor.typeItem &&!downCheck.isCheckedVertical )
        {
            if (downCheck.posMatrix != firstItemInColumn.posMatrix)
            {
                firstItemInColumn.isCheckedVertical = true;
                if (!matchingList.Contains(firstItemInColumn))
                    matchingList.Add(firstItemInColumn);
                if (!matchingList.Contains(downCheck))
                    matchingList.Add(downCheck);
                Check(downCheck);
            }
        }
    }
}