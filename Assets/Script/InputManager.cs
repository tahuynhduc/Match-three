using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float swipeThreshold;

    private BoardManager BoardManager => BoardManager.Instance;
    private bool _isSelectedChess,_isSwiping;
    private Vector2 _startTouchPosition, _endTouchPosition;
    private void Start()
    {
        _isSelectedChess = false;
        _isSwiping = false;
    }

    void Update()
    {
        SelectedItem();
        DetectSwipe();
    }

    private void DetectSwipe()
    {

        if (Input.GetMouseButtonUp(0) && _isSwiping)
        {
            if(!_isSelectedChess) return;
            _endTouchPosition = Input.mousePosition;
            DetectSwipeDirection();
            _isSwiping = false;
        }

        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                _startTouchPosition = touch.position;
                break;

            case TouchPhase.Ended:
                _endTouchPosition = touch.position;
                DetectSwipeDirection();
                break;
        }
    }

    private void DetectSwipeDirection()
    {
        var swipeDirection = _endTouchPosition - _startTouchPosition;

        if (!(swipeDirection.magnitude >= swipeThreshold)) return;
        var x = swipeDirection.x;
        var y = swipeDirection.y;
        TouchDirection direction;
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            direction = x > 0 ? TouchDirection.Right : TouchDirection.Left;
        }
        else
        {
            direction = y > 0 ? TouchDirection.Top : TouchDirection.Down;
        }
        BoardManager.GetNearItem(direction);
    }
    private void SelectedItem()
    {
        if(_isSwiping) return;
        if (!Input.GetMouseButton(0)) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return;
        var item = hit.collider.gameObject.GetComponent<Item>();
        
        if(item == null) return;
        _startTouchPosition = Input.mousePosition;
        _isSwiping = true;
        EventManager.OnSelectedItem(item);
        _isSelectedChess = true;
    }
}

public enum TouchDirection
{
    None =0,
    Top =1,
    Down =2,
    Left =3,
    Right =4
}