using System;
using System.Collections.Generic;

[Serializable]
public class Row<T>
{
    public List<T> rowItem = new();
}