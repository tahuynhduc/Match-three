using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Col<T>
{
    public List<Row<T>> colItem = new();
}