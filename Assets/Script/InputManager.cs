using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    public static TouchDirection direction;
    
    [SerializeField] private float swipeThreshold;

    private BoardManager BoardManager => BoardManager.Instance;
    private bool _isSelectedChess,_isSwiping,_touchState;
    private Vector2 _startTouchPosition, _endTouchPosition;

    private void OnEnable()
    {
        EventManager.SwapItem += UpdateStateTouch;
    }

    private void UpdateStateTouch(bool state)
    {
        _touchState = state;
    }
    private void Start()
    {
        _isSelectedChess = false;
        _isSwiping = false;
        _touchState = true;
    }
    
    void Update()
    {
        if(!_touchState) return;
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