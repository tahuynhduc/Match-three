using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : TemporaryMonoSingleton<PrefabsManager>
{
    public List<Type> colorItem;
    public GameObject prefab;

    public Type GetColor()
    {
        return this.RandomRange(colorItem);
    }
}
