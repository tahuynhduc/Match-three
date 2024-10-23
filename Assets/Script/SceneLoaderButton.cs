using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void OnClickLoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnEnable()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClickLoadScene);
    }
}
