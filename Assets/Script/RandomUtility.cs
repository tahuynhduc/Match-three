using UnityEngine;

public static class RandomUtility<T>
{
    public static T RandomEnum(object start,object end)
    {
        object result = Random.Range((int)start, (int)end);
        return (T)result;
    }
}