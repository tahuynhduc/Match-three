using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{
    [SerializeField] private List<ColorItem> colorItem;
    [SerializeField] private MeshRenderer meshRenderer;

    private Dictionary<Type, ColorItem> _dictColorItem;
     public ColorItem currentColor;
    
    //with xPos = col, yPos = row;
    public Vector2 posMatrix;
    public bool isCheckedHorizontal;
    public bool isCheckedVertical;
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    private void Awake()
    {
        _dictColorItem = new Dictionary<Type, ColorItem>();
        foreach (var item in colorItem)
        {
            _dictColorItem.Add(item.typeItem,item);
        }
    }
    public void UpdateColor(Type newColor)
    {
        meshRenderer.materials[0].color = _dictColorItem[newColor].colorItem;
        currentColor = _dictColorItem[newColor];
    }
    public void UpdatePosition(int x,int y)
    {
        posMatrix.x = x;
        posMatrix.y = y;
    }

    public void MoveDown(Vector2 pos)
    {
        transform.position = pos;
    }
    public void ActiveItem(bool state= false)
    {
        // Debug.LogError($"pos:{posMatrix}");
        gameObject.SetActive(state);
        // Destroy(gameObject);
    }
}
[Serializable]
public class ColorItem
{
    public Type typeItem;
    public Color colorItem;
}
public enum Type
{
    Red =0,
    Black =1,
    Blue =2,
    Green = 3,
    Cyan =4,
    Grey = 5,
    White =6,
    Yellow =7,
}