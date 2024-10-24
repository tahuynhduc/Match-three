using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBoardButton : MonoBehaviour
{
    private PrefabsManager PrefabsManager => SingletonManager.PrefabsManager;
    public void RandomColor()
    {
        EventManager.OnResetBoard();
    }
}
